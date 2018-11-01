using Newtonsoft.Json;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.Dto.Response
{
    /// <summary>
    /// 接口返回格式（不带数据的）
    /// </summary>
    public class ResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ResponseDto(StatusCodeEnum status = StatusCodeEnum.Ok, string message = "成功")
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCodeEnum Status { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 性能
        /// </summary>
        [JsonIgnore]
        public object Performance { get; set; }
    }
}
