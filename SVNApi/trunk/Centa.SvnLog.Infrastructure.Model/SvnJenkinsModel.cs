using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table("[dbo].[SVN_Jenkins]")]
    public class SvnJenkinsModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ExplicitKey]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectRelationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}