using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Dto.Request.ProjectRelation;
using Centa.SvnLog.Dto.Request.SvnJenkins;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/revision-publish")]
    [TokenValidate]
    public  class RevisionPublishController : Controller
    {
        private readonly IRevisionPublishService _revisionPublishService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="projectRelationService"></param>
        public RevisionPublishController(IRevisionPublishService projectRelationService)
        {
            _revisionPublishService = projectRelationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        public IActionResult GetRevisionPublishList([FromQuery] RevisionPublishRequestDto query, TokenModel tokenModel)
        {
            var list = _revisionPublishService.GetRevisionPublishList(query.LogID);
            return Ok(new PageResponseDto() { Data = list });
        }

        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddRevisionPublish([FromQuery] RevisionPublishAddRequestDto query, TokenModel tokenModel)
        {
            var result=_revisionPublishService.AddRevisionPublish(new SvnRevisionPublishModel()
            {
                LogID=query.LogID,
                SystemAccountID=tokenModel.ID,
                JenkinsID=query.JenkinsID,
                Description=query.Description
            });
            return Ok(new ResponseDto(result.Success? StatusCodeEnum.Ok: StatusCodeEnum.InternalError,result.Message));
        }
    }
}