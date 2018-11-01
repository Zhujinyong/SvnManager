using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Centa.SvnLog.Dto.Request.SvnUser;
using System;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Dto.Request.Account;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 系统用户接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/systemuser")]
    [TokenValidate]
    public  class SystemAccountController: Controller
    {
        private readonly ISystemUserService _systemUserService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="systemUserService"></param>
        public SystemAccountController(ISystemUserService systemUserService)
        {
            _systemUserService = systemUserService;
        }

        /// <summary>
        /// 系统用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        public IActionResult GetSvnUserList([FromQuery] SystemUserRequestDto query,TokenModel tokenModel)
        {
            var list = _systemUserService.GetSystemUserList<dynamic>(query.RealName, query.UserName, query.PageIndex, query.PageSize);
            return Ok(new PageResponseDto() { Data = list.Items, Page = new PageDto { PageSize = query.PageSize, Total = list.TotalNum, PageIndex = query.PageIndex } });
        }

        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddUser([FromQuery] SystemUserAddRequestDto query, TokenModel tokenModel)
        {
            _systemUserService.AddUser(new SystemUserModel()
            {
                RealName = query.RealName,
                UserName = query.UserName,
                Description = query.Description,
                Picture=query.Picture,
                IsUse=query.IsUse,
                ID = Guid.NewGuid().ToString().ToUpper()
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("update")]
        public IActionResult UpdateUser([FromQuery] SystemUserUpdateRequestDto query, TokenModel tokenModel)
        {
            _systemUserService.UpdateUser(new SystemUserModel()
            {
                ID = query.ID,
                RealName = query.RealName,
                UserName = query.UserName,
                Description = query.Description,
                Picture = query.Picture,
                IsUse = query.IsUse
            });
            return Ok(new ResponseDto());
        }

    }
}