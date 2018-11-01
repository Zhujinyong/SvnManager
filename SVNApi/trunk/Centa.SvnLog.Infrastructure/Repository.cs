using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Centa.SvnLog.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IDbConnection Connection
        {
            get
            {
                return Context.Connection;
            }
        }

        /// <summary>
        /// 存储appsetting.json里的数据库连接字符串
        /// </summary>
        private string _connectionString;

        public IDapperContext Context { get; private set; }

        public Repository(IDapperContext context)
        {
            Context = context;
            _connectionString =context.Connection.ConnectionString;
        }

        /// <summary>
        /// 设置上下文连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        public void SetContext(string connectionString)
        {
            IDapperContext context=new DapperContext(connectionString);
            Connection.Dispose();
            Context = context;
        }
        
        /// <summary>
        /// 重置上下文，用appSetting.json里的数据库连接串
        /// </summary>
        public void ResetContext()
        {
            SetContext(_connectionString);
        }

        public PageDataView<TEntity> GetPageData<TEntity>(PageCriteria criteria, object param = null) where TEntity : class
        {
            var p = new DynamicParameters();
            string proName = "CCHRWebApiProcGetPageData";
            p.Add("TableName", criteria.TableName);
            p.Add("PrimaryKey", criteria.PrimaryKey);
            p.Add("Fields", criteria.Fields);
            p.Add("Condition", criteria.Condition);
            p.Add("CurrentPage", criteria.CurrentPage);
            p.Add("PageSize", criteria.PageSize);
            p.Add("Sort", criteria.Sort);
            p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var pageData = new PageDataView<TEntity>();
            int excuteTimes = 0;
            //QUERY:
            try
            {
                pageData.Items = Connection.Query<TEntity>(proName, p, commandType: CommandType.StoredProcedure, commandTimeout:300).ToList();
                pageData.TotalNum = p.Get<int>("RecordCount");
                pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
                pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
            }
            catch (Exception e)
            {
                excuteTimes++;
                if (e.Message.Contains("找不到存储过程 'CCHRWebApiProcGetPageData'") && excuteTimes <= 3)
                {
                    FileStream fileStream = new FileStream("CCHRWebApiProcGetPageData.sql", FileMode.Open);
                    StreamReader sr = new StreamReader(fileStream);
                    string sql = sr.ReadToEnd();
                    if (fileStream != null)
                        fileStream.Close();
                    if (sr != null)
                        sr.Close();
                    if (!string.IsNullOrEmpty(sql))
                    {
                        Connection.Execute(sql);
                       // goto QUERY;
                    }
                    throw;
                }
                else if ((e.Message.Contains("远程主机强迫关闭了一个现有的连接") || e.Message.Contains("指定的网络名不再可用")) && excuteTimes <= 3)
                {
                    System.Threading.Thread.Sleep(excuteTimes * 1000);
                    //goto QUERY;
                }
                else
                {
                    throw;
                }
            }
            return pageData;
        }

        public long Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            var res = Connection.Insert(entity, transaction: transaction, commandTimeout: commandTimeout);
            return res;
        }

        public virtual void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }
            Connection.Update(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            Connection.Delete(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual T GetByKey(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return Connection.Get<T>(id, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Connection.GetAll<T>(transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Connection.Query<T>(where.ToString(), commandTimeout:commandTimeout);
        }

        public IEnumerable<dynamic> Query(string sql, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Connection.Query<dynamic>(sql);
        }

        public int Excute(string sql, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Connection.Execute(sql, transaction: transaction);
        }

        public PageDataView<TReturn> GetJoinPageData<TFirst, TSecond, TThird,TReturn>(PageCriteria criteria, Func<TFirst, TSecond, TThird, TReturn> map,string splitOnString) where TReturn : class
        {
            var p = new DynamicParameters();
            string proName = "CCHRWebApiProcGetPageData";
            p.Add("TableName", criteria.TableName);
            p.Add("PrimaryKey", criteria.PrimaryKey);
            p.Add("Fields", criteria.Fields);
            p.Add("Condition", criteria.Condition);
            p.Add("CurrentPage", criteria.CurrentPage);
            p.Add("PageSize", criteria.PageSize);
            p.Add("Sort", criteria.Sort);
            p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var pageData = new PageDataView<TReturn>();
            pageData.Items = Connection.Query<TFirst, TSecond, TThird, TReturn>(proName, map, p, splitOn: splitOnString, commandType: CommandType.StoredProcedure, commandTimeout: 300).ToList();
            pageData.TotalNum = p.Get<int>("RecordCount");
            pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
            pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
            return pageData;
        }
    }
}