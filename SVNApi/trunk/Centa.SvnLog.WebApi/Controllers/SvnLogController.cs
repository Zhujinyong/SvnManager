using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Request;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Centa.SvnLog.Dto.Response.Department;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Dto.Request.SvnUser;
using Centa.SvnLog.Dto.Request.SvnJenkins;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 项目接口
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/log")]
    //[SystemLog]
    [TokenValidate]
    public  class SvnLogController : Controller
    {
        private readonly ISvnLogService _svnLogService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="svnLogService"></param>
        public SvnLogController(ISvnLogService svnLogService)
        {
            _svnLogService = svnLogService;
        }

        /// <summary>
        /// svn日志列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("list")]
        [SwaggerResponse(200, Type = typeof(SvnLogModel))]
        public IActionResult GetSvnLogList([FromQuery] SvnLogRequestDto query,TokenModel tokenModel)
        {
            var list = _svnLogService.GetSvnLogList(query.PageIndex, query.PageSize, query.BeginTime, query.EndTime, query.ProjectID, query.UserID);
            return Ok(new PageResponseDto() { Data = list.Items, Page = new PageDto { PageSize = query.PageSize, Total = list.TotalNum, PageIndex = query.PageIndex } });
        }


        /// <summary>
        /// svn文件列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("files")]
        [SwaggerResponse(200, Type = typeof(SvnLogModel))]
        public IActionResult GetFileList([FromQuery] SvnLogFileRequestDto query, TokenModel tokenModel)
        {
            var list = _svnLogService.GetFileList(query.LogID);
            return Ok(new PageResponseDto() { Data = list.Items, Page = new PageDto { PageSize = 1000, Total = list.TotalNum, PageIndex = list.CurrentPage } });
        }

        /// <summary>
        /// 文件变更
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("file-change")]
        public IActionResult GetFileChange([FromQuery] SvnLogFileChangeRequestDto query, TokenModel tokenModel)
        {
            var list = _svnLogService.GetFileChange(query.LogFileID);
            return Ok(new DataResponseDto() { Data = list });
        }

        /// <summary>
        /// 文件内容
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("file-content")]
        public IActionResult GetFileContent([FromQuery] SvnLogFileChangeRequestDto query, TokenModel tokenModel)
        {
            var list = _svnLogService.GetFileContent(query.LogFileID);
            return Ok(new DataResponseDto() { Data = list });
        }


        /// <summary>
        /// 版本发布
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("publish-revision")]
        public IActionResult PublishRevision([FromQuery] SvnJenkinsRequestDto query, TokenModel tokenModel)
        {
           // var result = _svnLogService.PublishRevision(query.LogID);
            return Ok(new ResponseDto() {  });
        }
    }
}