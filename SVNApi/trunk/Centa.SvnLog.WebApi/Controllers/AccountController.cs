using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Request.Account;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Token;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Centa.SvnLog.Dto.Response.Account;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/account")]
    public  class AccountController : Controller
    {
        private readonly ITokenVerify _tokenService;

        private readonly IAccountService _accountService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="accountService"></param>
        public AccountController(ITokenVerify tokenService,  IAccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost("token")]
        public IActionResult GetToken([FromBody]LoginRequestDto login)
        {
            var state = _accountService.Login(login.UserName, login.Password, login.IsDomainAccount);
           // var state =1;
            if (state == -1)
            {
                return Ok(new ResponseDto(StatusCodeEnum.AccountError, "账号不存在"));
            }
            if (state == -2)
            {
                return Ok(new ResponseDto(StatusCodeEnum.AccountError,"账号密码错误"));
            }
            var tokenString = _tokenService.WriteToken(new AccountModel() { UserName= login.UserName });
            return Ok(new DataResponseDto() { Data = new TokenResponseDto { Token = tokenString, ExpireIn = 60 * 60 } });
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("infomation")]
        [SwaggerResponse(200, Type = typeof(AccountResponseDto))]
        public IActionResult GetUserInfo(TokenModel tokenModel)
        {
            var account = _accountService.GetAccount(tokenModel.UserName);
            return Ok(new DataResponseDto() { Data = new AccountResponseDto
            {
                RealName = account.RealName,
                Picture = account.Picture
            } });
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            return Ok(new ResponseDto() );
        }

    }
}