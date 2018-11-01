using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Centa.SvnLog.Dto.Response.Department;
using Centa.SvnLog.Dto.Request.Project;
using Centa.SvnLog.Infrastructure.Model;
using System;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 项目接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/project")]
    [TokenValidate]
    public  class ProjectController : Controller
    {
        private readonly IProjectService _svnLogService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="svnLogService"></param>
        public ProjectController(IProjectService svnLogService)
        {
            _svnLogService = svnLogService;
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        [SwaggerResponse(200, Type = typeof(DepartmentResponseDto))]
        public IActionResult GetProjectList([FromQuery] ProjectRequestDto query,TokenModel tokenModel)
        {
            var list = _svnLogService.GetProjectList<dynamic>(query.Name,query.PageIndex, query.PageSize);
            return Ok(new PageResponseDto() { Data = list.Items, Page = new PageDto { PageSize = query.PageSize, Total = list.TotalNum, PageIndex = query.PageIndex } });
        }

        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddProject([FromQuery] ProjectAddRequestDto query, TokenModel tokenModel)
        {
            _svnLogService.AddProject(new SvnProjectModel()
            {
                ID = Guid.NewGuid().ToString().ToUpper(),
                Name = query.Name,
                Url = query.Url,
                Type = query.Type,
                Description=query.Description,
                Head=query.Head,
                City=query.City,
                IsUse=query.IsUse,
                Creator=query.Creator
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("update")]
        public IActionResult UpdateProject([FromQuery] ProjectUpdateRequestDto query, TokenModel tokenModel)
        {
            _svnLogService.UpdateProject(new SvnProjectModel()
            {
                ID = query.ID,
                Name = query.Name,
                Url = query.Url,
                Type = query.Type,
                Description = query.Description,
                Head = query.Head,
                City = query.City,
                IsUse = query.IsUse,
                Creator = query.Creator
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("delete")]
        public IActionResult DeleteProject([FromQuery] ProjectDeleteRequestDto query, TokenModel tokenModel)
        {
            _svnLogService.DeleteProject(query.ID);
            return Ok(new ResponseDto());
        }
    }
}