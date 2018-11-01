using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.Account
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SystemUserRequestDto: QueryPageDateRequestDto
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [FromQuery(Name = "realName")]
        public string RealName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FromQuery(Name = "userName")]
        public string UserName { get; set; }
    }
}
