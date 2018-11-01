using Centa.SvnLog.Infrastructure.Model.Enum;
using System;

namespace Centa.SvnLog.WebApi.ModelBinder
{
    /// <summary>
    /// token模型，注意：属性名称不要和action,controller参数相同
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ValidateTo { get; set; }


        /// <summary>
        /// 是否启用，1启用，0禁用
        /// </summary>
        public StateEnum IsUse { get; set; }
    }
}
