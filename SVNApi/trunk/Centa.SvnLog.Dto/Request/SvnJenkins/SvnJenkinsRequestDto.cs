using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnJenkins
{
    /// <summary>
    /// 
    /// </summary>
    public class SvnJenkinsRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "projectRelationID")]
        public string ProjectRelationID { get; set; }
        
    }
}
