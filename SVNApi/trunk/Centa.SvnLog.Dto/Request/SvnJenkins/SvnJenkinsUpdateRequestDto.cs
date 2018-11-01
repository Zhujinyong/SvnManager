using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 项目关系
    /// </summary>
    public class SvnJenkinsUpdateRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "projectRelationID")]
        public string ProjectRelationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "jobName")]
        public string JobName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [FromQuery(Name = "description")]
        public string Description { get; set; }
    }
}
