using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnUser
{
    /// <summary>
    /// svn用户
    /// </summary>
    public class SvnUserRequestDto: QueryPageDateRequestDto
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [FromQuery(Name = "realName")]
        public string RealName { get; set; }

        /// <summary>
        /// 域账号
        /// </summary>
        [FromQuery(Name = "domainAccount")]
        public string DomainAccount { get; set; }
    }
}
