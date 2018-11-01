using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnJenkins
{
    /// <summary>
    /// 
    /// </summary>
    public class RevisionPublishRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "logID")]
        public string LogID { get; set; }
        
    }
}
