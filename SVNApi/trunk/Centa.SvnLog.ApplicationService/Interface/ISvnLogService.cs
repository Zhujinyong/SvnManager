using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// svn日志
    /// </summary>
    public interface ISvnLogService
    {
        PageDataView<SvnLogModel> GetSvnLogList(int pageIndex, int pageSize,string beginTime,string endTime,string projectID,string userID);

        PageDataView<SvnLogFileModel> GetFileList(string logID);

        string GetFileChange(string logFileID);

        string GetFileContent(string logFileID);

        bool PublishRevision(string logID);
    }
}
