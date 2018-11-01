using Centa.SvnLog.Infrastructure.Model.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Centa.SvnLog.Dto.Request.Account
{
    /// <summary>
    /// 登陆获取token
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [FromQuery(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [FromQuery(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// 是否域账号
        /// </summary>
        //[Required]
        [FromQuery(Name = "isDomainAccount")]
        public StateEnum IsDomainAccount { get; set; } = StateEnum.Yes;
    }
}
