using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// Svn项目关系
    /// </summary>
    [Table("[dbo].[SVN_ProjectRelation]")]
    public class SvnProjectRelationModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ChildID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentID { get; set; }

       
    }


   
}