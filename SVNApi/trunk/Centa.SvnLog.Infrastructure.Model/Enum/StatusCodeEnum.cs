using System.ComponentModel;

namespace Centa.SvnLog.Infrastructure.Model.Enum
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StatusCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Ok = 200,

        /// <summary>
        /// 内部错误
        /// </summary>
        [Description("内部错误")]
        InternalError = 500,

        /// <summary>
        /// 公司ID或凭证密钥错误
        /// </summary>
        [Description("公司ID或凭证密钥错误")]
        AccountError = 801,

        /// <summary>
        /// token过期
        /// </summary>
        [Description("token过期")]
        TokenExpired = 802,

        /// <summary>
        /// token无效
        /// </summary>
        [Description("token无效")]
        TokenInvalid = 803,

        /// <summary>
        /// 用户被禁用
        /// </summary>
        [Description("用户被禁用")]
        InvalidUser = 804,


        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParamInvalid = 805,

        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("没有权限")]
        UnAuthority = 806
    }
}