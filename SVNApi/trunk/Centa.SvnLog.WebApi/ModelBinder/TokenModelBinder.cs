using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.WebApi.Token;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Centa.SvnLog.WebApi.ModelBinder
{
    /// <summary>
    /// 模型绑定，解析query的token
    /// </summary>
    public class TokenModelBinder : IModelBinder
    {
        private readonly ITokenVerify _tokenService;

        private readonly IAccountService _accountService;

        public TokenModelBinder(ITokenVerify tokenService, IAccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        /// <summary>
        /// 请求header里传递参数token
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //参数必须包含token
            if (!(bindingContext.ActionContext.HttpContext.Request.Headers.ContainsKey("token")))
                return Task.CompletedTask;
            var token = bindingContext.ActionContext.HttpContext.Request.Headers["token"];
            //  解析token
            TokenModel result;
            try
            {
                result=_tokenService.ReadToken(token);
            }
            catch(Exception e)
            {
                return Task.CompletedTask;
            }
            
            #region 如果后台禁用用户，需要更新模型绑定相关字段
            var account = _accountService.GetAccount(result.UserName);
            if (account == null)
            {
                return Task.CompletedTask;
            }
            result.ID = account.ID;
            result.IsUse = account.IsUse;
            #endregion
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
