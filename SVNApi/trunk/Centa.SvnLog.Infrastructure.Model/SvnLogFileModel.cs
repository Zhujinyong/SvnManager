using System;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// Svn文件日志
    /// </summary>
    [Table("[dbo].[SVN_LogFile]")]
    public class SvnLogFileModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Action { get; set; }

    }
}