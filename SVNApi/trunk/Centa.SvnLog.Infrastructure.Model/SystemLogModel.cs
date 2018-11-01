using System;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Table("[dbo].[SystemLog]")]
    public class SystemLogModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ExplicitKey]
        public string ID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string UserIP { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 日志类型,1普通日志,2错误日志
        /// </summary>
        public LogTypeEnum LogType { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExcuteMilliseconds { get; set; }
    }
}