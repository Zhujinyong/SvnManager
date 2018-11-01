using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.Project
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectRequestDto:QueryPageDateRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
