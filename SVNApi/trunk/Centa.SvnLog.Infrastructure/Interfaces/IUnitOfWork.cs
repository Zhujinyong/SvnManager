using System;
using System.Collections.Generic;
using System.Data;

namespace Centa.SvnLog.Infrastructure.Interfaces
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action"></param>
        void RunTransaction(Action<IDbTransaction> action);

        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string sql);

        /// <summary>
        /// 设置上下文连接字符串
        /// </summary>
        /// <param name="connectionString">字符串</param>
        void SetContext(string connectionString);

        int Excute(string sql);
    }
}