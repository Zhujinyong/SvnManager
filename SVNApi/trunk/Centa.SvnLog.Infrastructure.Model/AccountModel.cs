using Centa.SvnLog.Infrastructure.Model.Enum;
using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 账号
    /// </summary>
    [Table("[dbo].[System_Account]")]
    public class AccountModel
    {
        public string ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否启用，1启用，0禁用
        /// </summary>
        public StateEnum IsUse { get; set; }

        /// <summary>
        /// 是否域账号
        /// </summary>
        public StateEnum IsDomainAccount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// token有限期，单位分钟
        /// </summary>
        public int TokenEffctiveMinute { get; set; }
    }
}