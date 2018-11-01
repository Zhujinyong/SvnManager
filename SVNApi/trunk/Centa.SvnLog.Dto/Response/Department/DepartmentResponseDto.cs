using System;

namespace Centa.SvnLog.Dto.Response.Department
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class DepartmentResponseDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string HROC { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 部门路径
        /// </summary>
        public string DeptPath { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否删除，1删除，0未删除
        /// </summary>
        public int FlagDeleted { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime DateOpen { get; set; }

        /// <summary>
        /// 删除日期
        /// </summary>
        public DateTime DateClosed { get; set; }

        /// <summary>
        /// 部门全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public string ParentID { get; set; }
    }
}