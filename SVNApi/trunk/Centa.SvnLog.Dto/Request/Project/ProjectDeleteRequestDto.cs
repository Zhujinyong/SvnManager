using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.Project
{
    /// <summary>
    /// 删除项目关系
    /// </summary>
    public class ProjectDeleteRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }
    }
}
