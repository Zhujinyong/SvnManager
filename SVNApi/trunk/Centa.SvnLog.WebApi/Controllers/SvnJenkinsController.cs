using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Dto.Request.ProjectRelation;
using Centa.SvnLog.Dto.Request.SvnJenkins;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/svn-jenkins")]
    [TokenValidate]
    public  class SvnJenkinsController : Controller
    {
        private readonly ISvnJenkinsService _svnJenkinsService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="projectRelationService"></param>
        public SvnJenkinsController(ISvnJenkinsService projectRelationService)
        {
            _svnJenkinsService = projectRelationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        public IActionResult GetSvnJenkinsList([FromQuery] SvnJenkinsRequestDto query, TokenModel tokenModel)
        {
            var list = _svnJenkinsService.GetSvnJenkinsList(query.ProjectRelationID);
            return Ok(new PageResponseDto() { Data = list });
        }

        [TokenValidate]
        [HttpPost("add")]
        public IActionResult AddSvnJenkins([FromQuery] SvnJenkinsAddRequestDto query, TokenModel tokenModel)
        {
            _svnJenkinsService.AddSvnJenkins(new SvnJenkinsModel()
            {
                ID = query.ID,
                Name=query.Name,
                JobName=query.JobName,
                Description=query.Description,
                ProjectRelationID=query.ProjectRelationID
            });
            return Ok(new ResponseDto());
        }

        [TokenValidate]
        [HttpPost("update")]
        public IActionResult UpdateSvnJenkins([FromQuery] SvnJenkinsUpdateRequestDto query, TokenModel tokenModel)
        {
            _svnJenkinsService.UpdateSvnJenkins(new SvnJenkinsModel()
            {
                ID = query.ID,
                Name = query.Name,
                JobName = query.JobName,
                Description = query.Description,
                ProjectRelationID = query.ProjectRelationID
            });
            return Ok(new ResponseDto());
        }

    }
}