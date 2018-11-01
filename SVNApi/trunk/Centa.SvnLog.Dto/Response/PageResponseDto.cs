
namespace Centa.SvnLog.Dto.Response
{
    /// <summary>
    /// 有数据的返回格式
    /// </summary>
    public class PageResponseDto : DataResponseDto
    {
        /// <summary>
        /// 分页
        /// </summary>
        public PageDto Page { get; set; }
    }

    /// <summary>
    /// 分页
    /// </summary>
    public class PageDto
    {
        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总共多少条数据
        /// </summary>
        public int Total { get; set; }
    }
}