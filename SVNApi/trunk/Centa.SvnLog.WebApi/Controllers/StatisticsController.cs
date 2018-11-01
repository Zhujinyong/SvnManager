using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Dto.Response;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Centa.SvnLog.Infrastructure.Model;
using System;
using Centa.SvnLog.Dto.Request.ProjectRelation;
using System.Linq;
using System.Collections.Generic;

namespace Centa.SvnLog.WebApi.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/svnlog/v{version:apiVersion}/statistics")]
    [TokenValidate]
    public  class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        /// <summary>
        /// IOC注入
        /// </summary>
        /// <param name="projectRelationService"></param>
        public StatisticsController(IStatisticsService projectRelationService)
        {
            _statisticsService = projectRelationService;
        }

        /// <summary>
        /// 项目树
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [TokenValidate]
        [HttpGet("treeList")]
        public IActionResult GetProjectTreeList([FromQuery] ProjectStatisticsRequestDto query, TokenModel tokenModel)
        {
            var treeList=_statisticsService.GetProjectList(query.IDs,query.BeginTime,query.EndTime);
            return Ok(new DataResponseDto() { Data=treeList});
        }

        [TokenValidate]
        [HttpGet("submitUser")]
        public IActionResult GetProjectSvnUserList([FromQuery] ProjectSubmitUserStatisticsRequestDto query, TokenModel tokenModel)
        {
            var treeList = _statisticsService.GetProjectSvnUserList(query.ID, query.BeginTime, query.EndTime);
            return Ok(new DataResponseDto() { Data = treeList });
        }


        [TokenValidate]
        [HttpGet("user")]
        public IActionResult GetSvnUserList([FromQuery] ProjectSubmitUserStatisticsRequestDto query, TokenModel tokenModel)
        {
            var treeList = _statisticsService.GetSvnUserList(query.ID, query.BeginTime, query.EndTime);
            return Ok(new DataResponseDto() { Data = treeList });
        }
    }
}