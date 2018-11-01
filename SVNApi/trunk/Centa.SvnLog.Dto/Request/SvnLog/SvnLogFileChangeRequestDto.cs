using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnUser
{
    /// <summary>
    /// 
    /// </summary>
    public class SvnLogFileChangeRequestDto
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [FromQuery(Name = "logFileID")]
        public string LogFileID { get; set; }
    }
}
