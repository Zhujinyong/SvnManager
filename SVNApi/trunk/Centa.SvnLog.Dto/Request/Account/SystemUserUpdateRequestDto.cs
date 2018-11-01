using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.Account
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SystemUserUpdateRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }

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

        /// <summary>
        /// 描述
        /// </summary>
        [FromQuery(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [FromQuery(Name = "isUse")]
        public int IsUse { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [FromQuery(Name = "picture")]
        public string Picture { get; set; }
    }
}
