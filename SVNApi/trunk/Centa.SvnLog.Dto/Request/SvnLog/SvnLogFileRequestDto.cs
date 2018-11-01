using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnUser
{
    /// <summary>
    /// svn日志文件
    /// </summary>
    public class SvnLogFileRequestDto
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [FromQuery(Name = "logID")]
        public string LogID { get; set; }
    }
}
