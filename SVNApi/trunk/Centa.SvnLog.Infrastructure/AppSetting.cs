
namespace Centa.SvnLog.Infrastructure
{
    /// <summary>
    /// 对应appsetting.json的AppSetting节点
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否使用MiniProfiler
        /// </summary>
        public bool UseMiniProfiling { get; set; }

        /// <summary>
        /// 管理员账号，英文逗号隔开
        /// </summary>
        public string Administrator { get; set; }

        /// <summary>
        /// svn服务地址
        /// </summary>
        public string SvnWebServiceUrl { get; set; }

        /// <summary>
        /// jenkins服务地址
        /// </summary>
        public string JenkinsUrl { get; set; }
    }
}