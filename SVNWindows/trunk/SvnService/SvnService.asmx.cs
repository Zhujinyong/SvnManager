using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using Centa.Svn;

namespace SvnService
{
    /// <summary>
    /// Summary description for SvnService
    /// </summary>
    [WebService(Namespace = "SvnService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SvnService : System.Web.Services.WebService
    {

        /// <summary>
        /// 获取svn提交日志
        /// </summary>
        /// <param name="startReversion">开始版本</param>
        /// <param name="endReversion">结束版本</param>
        /// <param name="userName">svn账号</param>
        /// <param name="password">svn密码</param>
        /// <param name="url">svn项目地址</param>
        [WebMethod]
        public void GetLogs(int startReversion = 170, int endReversion = 172, string userName = "zhujy7", string password = "Zjy123456", string url = "https://tjs-svn.centaline.com.cn:8443/svn/cchr/cchrwebapi/trunk/sourcecode")
        {
            var svn = new SvnHelper() { UserName = userName, Password = password };
            var list = svn.GetLogs(url, startReversion, endReversion);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Context.Response.Write(JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 获取svn文件变更日志
        /// </summary>
        /// <param name="startReversion">开始版本</param>
        /// <param name="endReversion">结束版本</param>
        /// <param name="userName">svn账号</param>
        /// <param name="password">svn密码</param>
        /// <param name="url">svn项目地址</param>
        [WebMethod]
        public void GetChangedFileList(int startReversion, int endReversion, string userName, string password , string url)
        {
            var svn = new SvnHelper() { UserName = userName, Password = password };
            var list = svn.GetChangedFileList(url, startReversion, endReversion);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Context.Response.Write(JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 变更内容
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        /// <param name="reversion"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetFileChange(string userName, string password, string fileUrl, int reversion, int beforeReversion)
        {
            var svn = new SvnHelper() { UserName = userName, Password = password };
            var result = new Result() { Content = svn.Diff(fileUrl, reversion, beforeReversion) };
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.Write(JsonConvert.SerializeObject(result));
        }


        [WebMethod]
        public void GetFileContent(string userName, string password, string fileUrl, int reversion)
        {
            var svn = new SvnHelper() { UserName = userName, Password = password };
            var result = new Result() { Content = svn.GetFileContent(fileUrl, reversion) };
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.Write(JsonConvert.SerializeObject(result));
        }

        private class Result
        {
            public string Content { get; set; }
        }
    }
}
