
namespace Centa.SvnLog.Dto.Response
{
    /// <summary>
    /// 单个数据的返回格式
    /// </summary>
    public class DataResponseDto : ResponseDto
    {
        /// <summary>
        /// 单个数据
        /// </summary>
        public object Data { get; set; }
    }
}
