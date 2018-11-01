using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 
    /// </summary>
    public class RevisionPublishAddRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "logID")]
        public string LogID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "jenkinsID")]
        public string JenkinsID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [FromQuery(Name = "description")]
        public string Description { get; set; }
    }
}
