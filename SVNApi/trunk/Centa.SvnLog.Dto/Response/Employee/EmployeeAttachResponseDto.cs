using System;

namespace Centa.SvnLog.Dto.Response.Employee
{
    /// <summary>
    /// 资料附表信息
    /// </summary>
    public class EmployeeAttachResponseDto
    {
        /// <summary>
        /// 职员ID
        /// </summary>
        public string EmpID { get; set; }

        /// <summary>
        /// 职员工号
        /// </summary>
        public string EmpNO { get; set; }

        /// <summary>
        /// 是否二次入职，1是0否
        /// </summary>
        public int FlagReJoin { get; set; }

        /// <summary>
        /// 是否离职，1是0否
        /// </summary>
        public int FlagDimission { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModDate { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }
    }
}