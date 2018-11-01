using Microsoft.AspNetCore.Mvc;

namespace Centa.SvnLog.Dto.Request.Project
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectUpdateRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [FromQuery(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [FromQuery(Name = "realName")]
        public string RealName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [FromQuery(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// 1源代码，2产品文档
        /// </summary>
        [FromQuery(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [FromQuery(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [FromQuery(Name = "head")]
        public string Head { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [FromQuery(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [FromQuery(Name = "isUse")]
        public int IsUse { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [FromQuery(Name = "creator")]
        public string Creator { get; set; }
    }
}
