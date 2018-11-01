using System.Collections.Generic;
using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Request;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Centa.SvnLog.Dto.Response.Department;
using Centa.SvnLog.Infrastructure.General.Helpers;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 公共接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/common")]
    public  class CommonController : Controller
    {
        /// <summary>
        /// IOC注入
        /// </summary>
        public CommonController()
        {
        }

        /// <summary>
        /// 错误码列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("error-codes")]
        public IActionResult GetErrorCode()
        {
            Dictionary<int, string> dic = (typeof(StatusCodeEnum)).GetEnumDescription();
            return Ok(new PageResponseDto() { Data = dic });
        }
    }
}