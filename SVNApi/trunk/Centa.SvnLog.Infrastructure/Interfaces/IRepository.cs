using Centa.SvnLog.Infrastructure.General.Page;
using System;
using System.Collections.Generic;
using System.Data;

namespace Centa.SvnLog.Infrastructure.Interfaces
{
    /// <summary>
    /// 数据库CRUD等操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 连接
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 上下文
        /// </summary>
        IDapperContext Context { get; }

        /// <summary>
        /// 设置上下文连接字符串
        /// </summary>
        /// <param name="connectionString">字符串</param>
        void SetContext(string connectionString);

        /// <summary>
        /// 重置上下文，用appSetting.json里的数据库连接串
        /// </summary>
        void ResetContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria">查询设置</param>
        /// <param name="param"></param>
        /// <returns></returns>
        PageDataView<TEntity> GetPageData<TEntity>(PageCriteria criteria, object param = null) where TEntity : class;

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        long Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        T GetByKey(object key, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据条件获取数据列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string sql, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Excute(string sql, IDbTransaction transaction = null, int? commandTimeout = null);

        PageDataView<TReturn> GetJoinPageData<TFirst, TSecond, TThird, TReturn>(PageCriteria criteria, Func<TFirst, TSecond, TThird, TReturn> map, string splitOnString) where TReturn : class;

    }
}