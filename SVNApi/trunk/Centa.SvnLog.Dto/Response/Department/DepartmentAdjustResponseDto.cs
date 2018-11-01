using System;

namespace Centa.SvnLog.Dto.Response.Department
{
    /// <summary>
    /// 部门调整单据
    /// </summary>
    public class DepartmentAdjustResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string RowId { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime DateBegin { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime DateEnd { get; set; }
    }
}