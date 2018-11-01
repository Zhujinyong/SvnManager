using System;
using System.Collections.Generic;
using System.Text;

namespace Centa.Svn
{
    public class SvnInfomation
    {
        /// <summary>
        /// svn地址
        /// </summary>
        public string SvnUrl { get; set; }

        /// <summary>
        /// svn账号
        /// </summary>
        public string UserName  { get; set; }

        /// <summary>
        /// svn密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 开始版本
        /// </summary>
        public int StartReversion { get; set; }

        /// <summary>
        /// 结束版本
        /// </summary>
        public int EndReversion { get; set; }
    }
}
