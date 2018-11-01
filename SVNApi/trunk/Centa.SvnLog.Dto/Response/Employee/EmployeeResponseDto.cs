using System;

namespace Centa.SvnLog.Dto.Response.Employee
{
    /// <summary>
    /// 职员信息
    /// </summary>
    public class EmployeeResponseDto
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
        /// 职员姓名
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string EmpTel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否删除，1删除，0未删除
        /// </summary>
        public int FlagDeleted { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 域账号
        /// </summary>
        public string DomainAccount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string EmpStatus { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime EmpModDate { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 加密后的身份证
        /// </summary>
        public string MD5IDCard18 { get; set; }

        /// <summary>
        /// 进入集团日期
        /// </summary>
        public DateTime InGroupDate { get; set; }

        /// <summary>
        /// 进入公司日期
        /// </summary>
        public DateTime InCompanyDate { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime DimissionDate { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public string PositionID { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 职位修改日期
        /// </summary>
        public DateTime EmpPositionModDate { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门路径
        /// </summary>
        public string DeptPath { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string HROC { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string DateBirth { get; set; }

        /// <summary>
        /// 最高学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }

        /// <summary>
        /// 户口所在地
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 是否部门负责人(1负责人，0非负责人)
        /// </summary>
        public int FlagPrincipal { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public string JobLevel { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string JobGrade { get; set; }
    }
}