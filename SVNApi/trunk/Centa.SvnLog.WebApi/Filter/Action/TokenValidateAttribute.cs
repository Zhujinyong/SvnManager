using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Centa.SvnLog.WebApi.Filter.Action
{
    /// <summary>
    /// token验证，加上模型绑定，不需要在每个Action里写
    /// </summary>
    public class TokenValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.HttpContext.Request.Headers.ContainsKey("token")))
            {
                context.Result = new OkObjectResult(new ResponseDto(StatusCodeEnum.ParamInvalid, "参数错误"));
                return;
            }
            if (!context.ActionArguments.ContainsKey("tokenModel") || !context.ActionArguments["tokenModel"].GetType().ToString().Contains("TokenModel"))
            {
                context.Result = new OkObjectResult(new ResponseDto(StatusCodeEnum.ParamInvalid, "参数错误"));
                return;
            }
            var result = context.ActionArguments["tokenModel"] as TokenModel;
            if (result == null || string.IsNullOrEmpty(result.UserName))
            {
                context.Result = new OkObjectResult(new ResponseDto(StatusCodeEnum.TokenInvalid, "token无效"));
                return;
            }
            if (result.ValidateTo < DateTime.Now)
            {
                context.Result = new OkObjectResult(new ResponseDto(StatusCodeEnum.TokenExpired, "token过期,请重新登录"));
                return;
            }
        }
    }
}
