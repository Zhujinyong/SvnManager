using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 版本发布
    /// </summary>
    [Table("[dbo].[SVN_RevisionPublish]")]
    public class SvnRevisionPublishModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 发布用户ID
        /// </summary>
        public string SystemAccountID { get; set; }

        /// <summary>
        /// 日志ID
        /// </summary>
        public string LogID { get; set; }

        /// <summary>
        /// 发布配置ID
        /// </summary>
        public string JenkinsID { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public int CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }


   
}