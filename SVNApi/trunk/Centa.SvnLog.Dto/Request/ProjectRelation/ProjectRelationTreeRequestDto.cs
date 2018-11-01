using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 项目树
    /// </summary>
    public class ProjectRelationTreeRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }
    }
}
