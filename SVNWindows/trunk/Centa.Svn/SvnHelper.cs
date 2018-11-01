using SharpSvn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.IO;

namespace Centa.Svn
{
    public class SvnHelper
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// svn日志
        /// </summary>
        /// <param name="url"></param>
        /// <param name="startReversion"></param>
        /// <param name="endReversion"></param>
        /// <returns></returns>
        public List<SvnReversionModel> GetLogs(string url, int startReversion, int endReversion)
        {
            List<SvnReversionModel> list = new List<SvnReversionModel>();
            using (SvnClient client = GetSvnClient())
            {
                Collection<SvnLogEventArgs> logs;

                SvnInfoEventArgs info;
                Uri repos = new Uri(url);
                SvnLogArgs svnLogArgs=null;
                if(startReversion == 0 && endReversion == 0)
                {
                    svnLogArgs = new SvnLogArgs();
                }
                else if(startReversion == 0 && endReversion != 0)
                {
                    svnLogArgs = new SvnLogArgs(new SvnRevisionRange(startReversion, endReversion));
                }
                else if (startReversion != 0 && endReversion == 0)
                {
                    client.GetInfo(repos, out info);
                    if (startReversion == info.LastChangeRevision)
                    {
                        //没有更新 
                        return list;
                    }
                    svnLogArgs = new SvnLogArgs(new SvnRevisionRange(startReversion+1, info.LastChangeRevision));
                }
                else 
                {
                    if (startReversion == endReversion)
                    {
                        //没有更新 
                        return list;
                    }
                     svnLogArgs =new SvnLogArgs(new SvnRevisionRange(startReversion,endReversion));
                }
                if (client.GetLog(repos, svnLogArgs, out logs))
                {
                    foreach (var log in logs)
                    {
                        var reversion = new SvnReversionModel()
                        {
                            Reversion = log.Revision,
                            Author = log.Author,
                            Time = log.Time.AddHours(8),
                            LogMessage = log.LogMessage,
                            PathList = new List<SvnPathMessage>()
                        };
                        if (log.ChangedPaths != null)
                        {
                            foreach (var path in log.ChangedPaths)
                            {
                                var svnPath = new SvnPathMessage()
                                {
                                    Path = path.Path,
                                    Action = path.Action.ToString(),
                                    Time = reversion.Time,
                                    Reversion = log.Revision,
                                    Author = log.Author,
                                    LogMessage = log.LogMessage
                                };
                                reversion.PathList.Add(svnPath);
                            }
                        }
                       
                        list.Add(reversion);
                    }
                }
                return list.OrderByDescending(p=>p.Time).ToList();
            }
        }

        /// <summary>
        /// svn变更文件列表
        /// </summary>
        /// <param name="url"></param>
        /// <param name="startReversion"></param>
        /// <param name="endReversion"></param>
        /// <returns></returns>
        public List<SvnFileMessage> GetChangedFileList(string url, int startReversion, int endReversion)
        {
           return GetChangedFileList(GetLogs(url, startReversion, endReversion));
        }

        /// <summary>
        /// svn变更日志，私有
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<SvnFileMessage> GetChangedFileList(List<SvnReversionModel> list)
        {
            List<SvnFileMessage> fileList = new List<SvnFileMessage>();
            List<SvnPathMessage> svnPathList = new List<SvnPathMessage>();
            foreach (var svnReversion in list)
            {
                if (svnReversion.PathList != null && svnReversion.PathList.Count > 0)
                {
                    svnPathList.AddRange(svnReversion.PathList);
                }
            }
            var groupList = svnPathList.OrderByDescending(p => p.Time).GroupBy(p => p.Path).Select(p => new SvnFileMessage
            {
                Path=p.Key,
                Time=p.FirstOrDefault().Time,
                Action=p.FirstOrDefault().Action,
                Reversion=p.FirstOrDefault().Reversion,
                ReversionList=p.ToList()
            }).ToList();
            return groupList;
        }

        private SvnClient GetSvnClient()
        {
            SvnClient client = new SvnClient();
            client.Authentication.Clear();
            client.Authentication.UserNamePasswordHandlers += Authentication_UserNamePasswordHandlers;
            client.Authentication.SslServerTrustHandlers += Authentication_SslServerTrustHandlers;
            return client;
        }

        private void Authentication_UserNamePasswordHandlers(object sender, SharpSvn.Security.SvnUserNamePasswordEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                throw new Exception("用户名或密码为空！");
            }
            e.UserName = UserName;
            e.Password = Password;
        }

        private void Authentication_SslServerTrustHandlers(object sender, SharpSvn.Security.SvnSslServerTrustEventArgs e)
        {
            e.AcceptedFailures = e.Failures;
            e.Save = true;
        }

        public string Diff(string path, int reversion, int beforeReversion)
        {
            var result = string.Empty;
            using (SvnClient client = GetSvnClient())
            {
                SvnUriTarget from = new SvnUriTarget(path, beforeReversion);
                SvnUriTarget to = new SvnUriTarget(path, reversion);
                MemoryStream stream = new MemoryStream();
                if (client.Diff(from, to, stream))
                {
                    stream.Position = 0;
                    StreamReader strReader = new StreamReader(stream);
                    result = strReader.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="reversion"></param>
        /// <returns></returns>
        public string GetFileContent(string path, int reversion)
        {
            var result = string.Empty;
            using (SvnClient client = GetSvnClient())
            {
                SvnUriTarget to = new SvnUriTarget(path, reversion);
                using (MemoryStream stream = new MemoryStream())
                {
                    client.Write(to, stream);
                    stream.Position = 0;
                    StreamReader strReader = new StreamReader(stream);
                    result = strReader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
