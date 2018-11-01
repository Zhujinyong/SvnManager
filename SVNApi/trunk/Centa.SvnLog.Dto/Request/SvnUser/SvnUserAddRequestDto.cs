using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.SvnUser
{
    /// <summary>
    /// svn用户
    /// </summary>
    public class SvnUserAddRequestDto
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

        /// <summary>
        /// 是否有效
        /// </summary>
        [FromQuery(Name = "isUse")]
        public int IsUse { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [FromQuery(Name = "description")]
        public string Description { get; set; }
    }
}
