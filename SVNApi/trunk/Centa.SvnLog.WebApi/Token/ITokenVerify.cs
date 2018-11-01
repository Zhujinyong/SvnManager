using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.WebApi.ModelBinder;

namespace Centa.SvnLog.WebApi.Token
{
    /// <summary>
    /// token的生成和解析
    /// </summary>
    public interface ITokenVerify
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string WriteToken(AccountModel user);

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        TokenModel ReadToken(string token);
    }
}