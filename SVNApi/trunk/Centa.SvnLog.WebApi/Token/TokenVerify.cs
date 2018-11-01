using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Centa.SvnLog.WebApi.Token
{
    public class TokenVerify : ITokenVerify
    {
        private readonly IConfiguration _configuration;

        public TokenVerify(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string WriteToken(AccountModel account)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.NameId, account.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //获取的Validate时间有问题，相差8小时
            var now = DateTime.Now;
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              notBefore: now,
              expires: now.AddMinutes(int.Parse(_configuration["Jwt:TokenEffctiveMinute"])),
              claims: claims,
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenModel ReadToken(string token)
        {
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var tokenModel = new TokenModel()
                {
                    UserName = jwtToken.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.NameId).Value,
                    ValidateTo = jwtToken.ValidTo.AddHours(8)
                };
                return tokenModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}