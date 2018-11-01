using System;

namespace Centa.SvnLog.Dto.Response.Employee
{
    /// <summary>
    /// 任职历史
    /// </summary>
    public class EmployeeAdjustHistoryResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string RowId { get; set; }

        /// <summary>
        /// 职员ID
        /// </summary>
        public string EmpID { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime InPositionDate { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime DimissionDate { get; set; }

        /// <summary>
        /// 主任职标记，1是0否
        /// </summary>
        public int FlagPrimary { get; set; }

        /// <summary>
        /// 进入集团日期
        /// </summary>
        public DateTime InGroupDate { get; set; }

        /// <summary>
        /// 进入公司日期
        /// </summary>
        public DateTime InCompanyDate { get; set; }

        /// <summary>
        /// 进入部门日期
        /// </summary>
        public DateTime InDeptDate { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 异动类型
        /// </summary>
        public string AdjustType { get; set; }

        /// <summary>
        /// 跑盘标记(1代表跑盘的任职历史，0代表正式入职的历史记录)
        /// </summary>
        public int FlagRun { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public int JobLevel { get; set; }

        /// <summary>
        /// 是否在职，1是0否
        /// </summary>
        public int IsOnDuty { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public string PositionID { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string HROC { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门路径
        /// </summary>
        public string DeptPath { get; set; }
    }
}