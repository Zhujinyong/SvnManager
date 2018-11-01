using System;
using System.Data;

namespace Centa.SvnLog.Infrastructure.Interfaces
{
    /// <summary>
    /// Dapper上下文
    /// </summary>
    public interface IDapperContext : IDisposable
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        IDbConnection Connection { get; }
    }
}