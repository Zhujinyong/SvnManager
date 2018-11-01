using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request
{
    /// <summary>
    /// 页码时间请求参数
    /// </summary>
    public class QueryPageDateRequestDto: QueryDateRangeDto
    {
        /// <summary>
        /// @Order=3,页码，默认值1，如果传值-1，会返回所有记录
        /// </summary>
        [FromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// @Order=4,每页多少条记录，默认10
        /// </summary>
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// 日期查询参数，开始日期和结束日期
    /// </summary>
    public class QueryDateRangeDto
    {
        /// <summary>
        /// @Order=5,开始修改时间，字符串，格式是："yyyy-MM-dd hh:mm:ss"，默认值"1900-01-01 00:00:00"
        /// </summary>
        [FromQuery(Name = "beginTime")]
        public string BeginTime { get; set; } = "1900-01-01 00:00:00";

        /// <summary>
        /// @Order=6,结束修改时间，字符串，格式是："yyyy-MM-dd hh:mm:ss"，默认值"9999-12-31 23:59:29"
        /// </summary>
        [FromQuery(Name = "endTime")]
        public string EndTime { get; set; } = "9999-12-31 23:59:29";
    }

}
