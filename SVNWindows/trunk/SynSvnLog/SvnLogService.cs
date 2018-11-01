using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SynSvnLog.Model;
using Centa.Svn;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;

namespace SynSvnLog
{
    public class SvnLogService
    {
        private List<SvnProject> _svnProjectList = new List<SvnProject>();

        private readonly string _connetionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        private readonly string _logPath = Application.StartupPath + ConfigurationManager.AppSettings.Get("LogPath");

        /// <summary>
        /// 用户缓存
        /// </summary>
        private List<SvnUser>_svnUserList = new List<SvnUser>();

        private static FileHelper fileHelper = new FileHelper();

        private void GetProjectRevision()
        {
            using (SqlConnection connection = new SqlConnection(_connetionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"select p.ID,p.Url,p.Name,u.DomainAccount,u.Password,c.Revision from SVN_Project as p  
                                        LEFT   JOIN (
                                        select b.ProjectID, max(Revision) as Revision from SVN_Log as b
                                        GROUP BY b.ProjectID)  as c on  p.id=c.ProjectID
                                        LEFT JOIN SVN_User as u on p.UserID=u.id
                                        where p.IsUse=1 and  (url like 'http%' or url like 'svn%');
                                        select ID,DomainAccount from SVN_User";
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (DataRow row in ds.Tables[0].AsEnumerable())
                {
                    SvnProject svnProject = new SvnProject();
                    svnProject.ID = row["ID"].ToString();
                    svnProject.Url = row["Url"].ToString();
                    svnProject.Name = row["Name"].ToString();
                    svnProject.DomainAccount = row["DomainAccount"].ToString();
                    svnProject.Password = row["Password"].ToString();
                    int revision = 0;
                    int.TryParse(row["Revision"].ToString(), out revision);
                    svnProject.Revision = revision;
                    _svnProjectList.Add(svnProject);
                }

                foreach (DataRow row in ds.Tables[1].AsEnumerable())
                {
                    SvnUser svnUser = new SvnUser();
                    svnUser.ID = row["ID"].ToString();
                    svnUser.DomainAccount = row["DomainAccount"].ToString();
                    _svnUserList.Add(svnUser);
                }
            }
        }

        public void UpdateSvnLogToDatabase()
        {
            GetProjectRevision();
            MessageAdd(string.Format("开始同步"));
            #region 遍历项目
            foreach (var svnProject in _svnProjectList)
            {
                MessageAdd(string.Format("项目：{0},数据库当前版本：{1}", svnProject.Name, svnProject.Revision));
                var svn = new SvnHelper() { UserName = svnProject.DomainAccount, Password = svnProject.Password };
                var logList = new List<SvnReversionModel>();
                try
                {
                    logList=svn.GetLogs(svnProject.Url, svnProject.Revision, 0);
                }
                catch (Exception e)
                {
                    MessageAdd(string.Format("出错了：{0},{1}", svnProject.Name, e.Message));
                    continue;
                }
               
                if (logList == null || logList.Count == 0)
                {
                    MessageAdd(string.Format("svn当前版本：{0}，无最新日志", svnProject.Revision));
                    continue;
                }
                //待插入的用户列表
                List<SvnUser> insertSvnUserList = new List<SvnUser>();
                //待插入的日志列表
                List<SvnLog> insertSvnLogList = new List<SvnLog>();
                //待插入的日志文件列表
                List<SvnLogFile> insertSvnLogFileList = new List<SvnLogFile>();

                #region 遍历日志 保存数据到insertSvnUserList、insertSvnLogList和insertSvnLogFileList
                foreach (var log in logList)
                {
                    SvnLog svnLog = new SvnLog();
                    svnLog.ID = Guid.NewGuid().ToString().ToUpper();
                    svnLog.ProjectID = svnProject.ID;
                    int revision = 0;
                    int.TryParse(log.Reversion.ToString(), out revision);
                    if (revision == 0)
                        continue;
                    svnLog.Revision = revision;
                    svnLog.CreateTime = log.Time;
                    svnLog.Message = log.LogMessage;
                    if (string.IsNullOrEmpty(log.Author))
                        continue;
                    var domainAccount=Regex.Replace(log.Author, "@CENTALINE", "", RegexOptions.IgnoreCase);
                    #region 用户是否存在，不存在，需要插入insertSvnUserList
                    if (_svnUserList.Any(p => p.DomainAccount == domainAccount))
                    {
                        svnLog.UserID = _svnUserList.FirstOrDefault(p => p.DomainAccount == domainAccount).ID;
                    }
                    else
                    {
                        var svnUser = new SvnUser()
                        {
                            ID = Guid.NewGuid().ToString().ToUpper(),
                            DomainAccount = domainAccount,
                            RealName = "导入用户" + new Random().Next(100, 999),
                            Description = "svn日志导入的用户，请修改真实姓名"
                        };
                        svnLog.UserID = svnUser.ID;
                        insertSvnUserList.Add(svnUser);
                        //插入用户缓存
                        _svnUserList.Add(svnUser);
                    }
                    #endregion
                    #region 插入日志insertSvnLogList和文件insertSvnLogFileList
                    insertSvnLogList.Add(svnLog);
                    foreach (var file in log.PathList)
                    {
                        SvnLogFile svnLogFile = new SvnLogFile();
                        svnLogFile.ID = Guid.NewGuid().ToString().ToUpper();
                        svnLogFile.LogID = svnLog.ID;
                        svnLogFile.Path = file.Path;
                        svnLogFile.Action = file.Action;
                        insertSvnLogFileList.Add(svnLogFile);
                    }
                    #endregion
                }
                #endregion

                #region 插入数据到表SVN_User、SVN_Log和SVN_LogFile
                //每次插入100条数据
                int insertCount = 800;
                StringBuilder sql = new StringBuilder();
                List<string> valueList = new List<string>();
                if (insertSvnUserList.Count > 0)
                {
                    for (int i = 0; i <= Math.Floor(insertSvnUserList.Count * 1.0 / insertCount); i++)
                    {
                        sql.Append(@"insert into SVN_User(ID,DomainAccount,RealName,Description) values");
                        insertSvnUserList.OrderBy(p=>p.ID).Skip(i * insertCount).Take(insertCount).ToList().ForEach(p =>
                        {
                            valueList.Add(string.Format("('{0}','{1}','{2}','{3}')", p.ID, p.DomainAccount, p.RealName, p.Description));
                        });
                        sql.Append(String.Join(",", valueList)+";");
                        ExcuteTransaction(sql.ToString());
                        sql.Clear();
                        valueList.Clear();
                    }
                }
                if (insertSvnLogList.Count > 0)
                {
                    for (int i = 0; i <= Math.Floor(insertSvnLogList.Count * 1.0 / insertCount); i++)
                    {
                        sql.Append(@"insert into SVN_Log(ID,ProjectID,UserID,CreateTime,Revision,Message) values");
                        insertSvnLogList.OrderBy(p => p.ID).Skip(i * insertCount).Take(insertCount).ToList().ForEach(p =>
                        {
                            valueList.Add(string.Format("('{0}','{1}','{2}','{3}',{4},'{5}')", p.ID, p.ProjectID, p.UserID, p.CreateTime, p.Revision, p.Message.Replace("//", "").Replace("'", "")));
                        });
                        sql.Append(String.Join(",", valueList) + ";");
                        ExcuteTransaction(sql.ToString());
                        sql.Clear();
                        valueList.Clear();
                    }
                }
                if (insertSvnLogFileList.Count > 0)
                {
                    for (int i =0; i <= Math.Floor(insertSvnLogFileList.Count * 1.0 / insertCount); i++)
                    {
                        sql.Append(@"insert into SVN_LogFile(LogID,Path,Action) values");
                        insertSvnLogFileList.OrderBy(p => p.LogID).Skip(i * insertCount).Take(insertCount).ToList().ForEach(p =>
                        {
                            valueList.Add(string.Format("('{0}','{1}','{2}')", p.LogID, p.Path, p.Action));
                        });
                        sql.Append(String.Join(",", valueList) + ";");
                        ExcuteTransaction(sql.ToString());
                        sql.Clear();
                        valueList.Clear();
                    }
                }

                //var sqlString = sql.ToString();
                //if (!string.IsNullOrEmpty(sqlString))
                //{
                //    ExcuteTransaction(sqlString);
                //}
                if (insertSvnLogList.Count > 0)
                {
                    MessageAdd(string.Format("更新了：{0}个版本：分别是：{1}", insertSvnLogList.Count, String.Join(",",insertSvnLogList.Select(p=>p.Revision).ToList())));
                }
                #endregion
            }
            #endregion
            MessageAdd(string.Format("结束同步\r\n\r\n"));
        }

        private void Save()
        {

        }
        private void ExcuteTransaction(string sql)
        {
            SqlConnection connection = new SqlConnection();  
            connection.ConnectionString = _connetionString;  
            connection.Open();  
            SqlTransaction transcation = connection.BeginTransaction();  
            SqlCommand command = new SqlCommand(); 
            command.Connection = connection;  
            command.Transaction = transcation; 
            try  
            {  
                command.CommandText = sql; 
                command.ExecuteNonQuery();   
                transcation.Commit(); 
            } 
            catch (Exception ex)  
            {  
                transcation.Rollback();
                MessageAdd("保存数据库失败：" + ex.Message);
            } 
            finally
            {  
                connection.Close();
            } 
        }

        public void MessageAdd(string message)
        {
            try
            {
                fileHelper.WriteLogFile(_logPath, message);
            }
            catch (Exception ex)
            {
                MessageAdd("写入记录日志错误：" + ex.Message);
            }
        }

    }
}
