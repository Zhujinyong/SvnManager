using System.Data;
using System.Data.SqlClient;
using Centa.SvnLog.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Centa.SvnLog.Infrastructure
{
    public class DapperContext : IDapperContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _connectionString;

        private bool _useMiniProfiling;

        /// <summary>
        /// 连接
        /// </summary>
        private IDbConnection _connection;

        /// <summary>
        /// 节点对象
        /// </summary>
        private readonly AppSetting _appSetting;

        /// <summary>
        /// 构造函数注入IOptions
        /// </summary>
        /// <param name="appSetting"></param>
        public DapperContext(IOptions<AppSetting> appSetting)
        {
            _appSetting = appSetting.Value;
            _connectionString = _appSetting.ConnectionString;
            _useMiniProfiling = _appSetting.UseMiniProfiling;
        }

        /// <summary>
        /// 连接字符串，用于各个公司
        /// </summary>
        /// <param name="connectionString"></param>
        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    if (_useMiniProfiling)
                    {
                        _connection = new ProfiledDbConnection(new SqlConnection(_connectionString), MiniProfiler.Current);
                    }
                    else
                    {
                        _connection = new SqlConnection(_connectionString);
                    }
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}