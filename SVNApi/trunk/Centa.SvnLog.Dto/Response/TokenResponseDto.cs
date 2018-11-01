
namespace Centa.SvnLog.Dto.Response
{
    /// <summary>
    /// token
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 有效期，单位秒
        /// </summary>
        public int ExpireIn { get; set; }
    }
}