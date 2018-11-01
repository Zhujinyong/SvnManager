using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.WebApi.Token;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Centa.SvnLog.WebApi.ModelBinder
{
    /// <summary>
    /// Provider
    /// </summary>
    public class TokenModelBinderProvider : IModelBinderProvider
    {
        private ITokenVerify _tokenService;

        private IAccountService _accountService;

        public TokenModelBinderProvider(ITokenVerify tokenService, IAccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(TokenModel))
                return new TokenModelBinder(_tokenService, _accountService);
            return null;
        }
    }
}