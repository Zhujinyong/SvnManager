using Centa.SvnLog.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Centa.SvnLog.Infrastructure.General.Exception;

namespace Centa.SvnLog.WebApi.Filter.Exception
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public CustomExceptionFilterAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            #region 自定义异常
            if (context.Exception.GetType()== typeof(CustomException))
            {
                context.Result = new JsonResult(new ResponseDto(StatusCodeEnum.ParamInvalid, context.Exception.ToString()));
            }
            #endregion
            #region 捕获程序异常，友好提示
            else
            {
                context.Result = new JsonResult(new ResponseDto(StatusCodeEnum.InternalError, "服务器忙，请稍后再试"));
            }
            #endregion
        }
    }
}
