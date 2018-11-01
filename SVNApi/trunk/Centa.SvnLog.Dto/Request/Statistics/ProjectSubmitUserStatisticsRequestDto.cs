using Microsoft.AspNetCore.Mvc;
using System;

namespace Centa.SvnLog.Dto.Request.ProjectRelation
{
    /// <summary>
    /// 项目统计查询->提交的用户
    /// </summary>
    public class ProjectSubmitUserStatisticsRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }

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
