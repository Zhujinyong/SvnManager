using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [Table("[dbo].[System_Account]")]
    public class SystemUserModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ExplicitKey]
        public string ID { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
    }
}