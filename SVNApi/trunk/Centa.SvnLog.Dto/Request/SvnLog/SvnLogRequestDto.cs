using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnUser
{
    /// <summary>
    /// svn日志
    /// </summary>
    public class SvnLogRequestDto:QueryPageDateRequestDto
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [FromQuery(Name = "projectID")]
        public string ProjectID { get; set; }

        /// <summary>
        /// svn用户ID
        /// </summary>
        [FromQuery(Name = "userID")]
        public string UserID { get; set; }
    }
}
