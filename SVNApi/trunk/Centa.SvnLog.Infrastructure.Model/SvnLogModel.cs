using System;
using System.Collections.Generic;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// Svn日志
    /// </summary>
    [Table("[dbo].[SVN_Log]")]
    public class SvnLogModel
    {
        public SvnLogModel()
        {
            SvnUser = new SvnUserModel();
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Message { get; set; }

        public DateTime CreateTime { get; set; }

        public SvnUserModel SvnUser { get; set; }

        public SvnProjectModel SvnProject { get; set; }

    }
}