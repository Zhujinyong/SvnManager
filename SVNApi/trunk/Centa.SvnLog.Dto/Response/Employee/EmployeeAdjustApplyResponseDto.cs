using System;

namespace Centa.SvnLog.Dto.Response.Employee
{
    /// <summary>
    /// 职员申请单
    /// </summary>
    public class EmployeeAdjustApplyResponseDto
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
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 事务类型
        /// </summary>
        public string AffairType { get; set; }

        /// <summary>
        /// 前部门ID
        /// </summary>
        public string PreDeptID { get; set; }

        /// <summary>
        /// 前职位ID
        /// </summary>
        public string PrePositionID { get; set; }

        /// <summary>
        /// 当前部门ID
        /// </summary>
        public string CurDeptID { get; set; }

        /// <summary>
        /// 当前职位ID
        /// </summary>
        public string CurPositionID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string FlagStatus { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime ValidedDate { get; set; }

        /// <summary>
        /// 职员编号
        /// </summary>
        public string EmpNO { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// AffairType取值异动的情况下，AdjustTypeID取值说明：1晋升，2降职，3转职
        /// </summary>
        public string AdjustTypeID { get; set; }

        /// <summary>
        /// 变动前职级
        /// </summary>
        public string PreJobLevel { get; set; }

        /// <summary>
        /// 变动前档位
        /// </summary>
        public string PreJobGrade { get; set; }

        /// <summary>
        /// 变动后职级
        /// </summary>
        public string CurJobLevel { get; set; }

        /// <summary>
        /// 变动后档位
        /// </summary>
        public string CurJobGrade { get; set; }
    }
}