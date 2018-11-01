using Centa.SvnLog.Infrastructure.Model.Enum;
using Dapper.Contrib.Extensions;

namespace Centa.SvnLog.Infrastructure.Model
{
    /// <summary>
    /// 公司设置
    /// </summary>
    [Table("[dbo].[CompanySetting]")]
    public class CompanySettingModel
    {
        /// <summary>
        /// 是否启用，1启用，0禁用
        /// </summary>
        public StateEnum IsUse { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司代码，如tj,sz
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 公司数据库连接字符串
        /// </summary>
        public string ConectionString { get; set; }

        /// <summary>
        /// 每次请求，一页最大多少条数据
        /// </summary>
        public int PageSize { get; set; }
    }
}