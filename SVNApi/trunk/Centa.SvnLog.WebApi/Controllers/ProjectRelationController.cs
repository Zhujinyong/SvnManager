using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Centa.SvnLog.Infrastructure.Model;
using System;
using Centa.SvnLog.Dto.Request.ProjectRelation;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 项目接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/project-relation")]
    [TokenValidate]
    public  class ProjectRelationController : Controller
    {
        private readonly IProjectRelationService _projectRelationService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="projectRelationService"></param>
        public ProjectRelationController(IProjectRelationService projectRelationService)
        {
            _projectRelationService = projectRelationService;
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("tree")]
        public IActionResult GetProjectTreeList([FromQuery] ProjectRelationRequestDto query, TokenModel tokenModel)
        {
            var list = _projectRelationService.GetProjectTreeList(query.ID);
            return Ok(new PageResponseDto() { Data = list });
        }

        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddProjectRelation([FromQuery] ProjectRelationAddRequestDto query, TokenModel tokenModel)
        {
            _projectRelationService.AddProjectRelation(new SvnProjectRelationModel()
            {
                ID = query.ID,
                ChildID = query.ChildID,
                ParentID = string.Equals(query.ParentID,"root")?null: query.ParentID
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("update")]
        public IActionResult UpdateProjectRelation([FromQuery] ProjectRelationUpdateRequestDto query, TokenModel tokenModel)
        {
            _projectRelationService.UpdateProjectRelation(new SvnProjectRelationModel()
            {
                ID = query.ID,
                ChildID = query.ChildID,
                ParentID = query.ParentID
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("delete")]
        public IActionResult DeleteProjectRelation([FromQuery] ProjectRelationDeleteRequestDto query, TokenModel tokenModel)
        {
            _projectRelationService.DeleteProjectRelation(query.ID);
            return Ok(new ResponseDto());
        }

        /// <summary>
        /// 项目树
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("treeList")]
        public IActionResult GetProjectTreeList([FromQuery] ProjectRelationTreeRequestDto query, TokenModel tokenModel)
        {
            var treeList=_projectRelationService.GetTreeList(query.ID);
            return Ok(new DataResponseDto() { Data=treeList});
        }
    }
}