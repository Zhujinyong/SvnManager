using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// Svn用户
    /// </summary>
    [Table("[dbo].[SVN_User]")]
    public class SvnUserModel
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
        /// 域账号
        /// </summary>
        public string DomainAccount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}