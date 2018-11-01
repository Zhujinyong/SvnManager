using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectRelationAddRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "childID")]
        public string ChildID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "parentID")]
        public string ParentID { get; set; }

    }
}
