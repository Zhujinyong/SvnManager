using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 项目统计查询
    /// </summary>
    public class ProjectStatisticsRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "ids")]
        public List<string> IDs { get; set; } = new List<string>();

        /// <summary>
        /// @Order=5,开始时间，字符串，格式是："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        [FromQuery(Name = "beginTime")]
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// @Order=6,结束时间，字符串，格式是："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }


    }
}
