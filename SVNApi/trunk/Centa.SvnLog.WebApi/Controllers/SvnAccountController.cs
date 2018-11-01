using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Request;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Centa.SvnLog.Dto.Response.Department;
using Centa.SvnLog.Dto.Request.SvnUser;
using Centa.SvnLog.Infrastructure.Model;
using System;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// svn用户接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/svnuser")]
    [TokenValidate]
    public  class SvnAccountController : Controller
    {
        private readonly ISvnUserService _svnUserService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="svnUserService"></param>
        public SvnAccountController(ISvnUserService svnUserService)
        {
            _svnUserService = svnUserService;
        }

        /// <summary>
        /// svn用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        public IActionResult GetSvnUserList([FromQuery] SvnUserRequestDto query,TokenModel tokenModel)
        {
            var list = _svnUserService.GetSvnUserList<dynamic>(query.RealName, query.DomainAccount, query.PageIndex, query.PageSize);
            return Ok(new PageResponseDto() { Data = list.Items, Page = new PageDto { PageSize = query.PageSize, Total = list.TotalNum, PageIndex = query.PageIndex } });
        }


        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddSvnUser([FromQuery] SvnUserAddRequestDto query, TokenModel tokenModel)
        {
             _svnUserService.AddSvnUser(new SvnUserModel() {
                RealName=query.RealName,
                IsUse=query.IsUse,
                DomainAccount=query.DomainAccount,
                Description=query.Description,
                ID=Guid.NewGuid().ToString().ToUpper()
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("update")]
        public IActionResult UpdateSvnUser([FromQuery] SvnUserUpdateRequestDto query, TokenModel tokenModel)
        {
             _svnUserService.UpdateSvnUser(new SvnUserModel()
            {
                ID=query.ID,
                IsUse=query.IsUse,
                RealName = query.RealName,
                DomainAccount = query.DomainAccount,
                Description = query.Description
            });
            return Ok(new ResponseDto());
        }

    }
}