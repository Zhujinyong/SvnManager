using System;

namespace Centa.SvnLog.Dto.Response.Position
{
    /// <summary>
    /// 职位信息
    /// </summary>
    public class PostionResponseDto
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 父职位ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int FlagDeleted { get; set; }

        /// <summary>
        /// 部门负责人职位标记(1负责人0非负责人)
        /// </summary>
        public int FlagPrincipal { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public string JobLevel { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string JobGrade { get; set; }

        /// <summary>
        /// 编制
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 部门直属职位
        /// </summary>
        public string FlagDeptDirect { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 职位路径
        /// </summary>
        public string Path { get; set; }
    }
}