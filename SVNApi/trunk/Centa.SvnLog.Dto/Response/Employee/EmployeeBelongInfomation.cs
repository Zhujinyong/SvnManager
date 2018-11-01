using System;

namespace Centa.SvnLog.Dto.Response.Employee
{
    /// <summary>
    /// 职员
    /// </summary>
    public class EmployeeBelongInfomation
    {
        /// <summary>
        /// 部门路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 部门全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptNo { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 职员ID
        /// </summary>
        public string EmpID { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public int JobLevel { get; set; }

        /// <summary>
        /// 部门负责人职位标记(1负责人0非负责人)
        /// </summary>
        public int FlagPrincipal { get; set; }

        /// <summary>
        /// 主任职标记，1是0否
        /// </summary>
        public int FlagPrimary { get; set; }

        /// <summary>
        /// 属于标记，1是0否
        /// </summary>
        public int IsBelongToDepartment { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }
    }
}