using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ISystemLogService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        long Add(SystemLogModel log);

        int UpdateExcuteTime(string id, long elapsedMilliseconds);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="url"></param>
        /// <param name="companyName">公司名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<SystemLogModel> GetList(string url, string companyName, LogTypeEnum? logType, int pageIndex, int pageSize);
    }
}
