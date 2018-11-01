using System;
using System.Collections.Generic;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// 统计
    /// </summary>
    public interface IStatisticsService
    {
        List<dynamic> GetProjectList(List<string> ids,DateTime? beginTime,DateTime? endTime);

        List<dynamic> GetSvnUserList(string id,DateTime? beginTime, DateTime? endTime);

        List<dynamic> GetProjectSvnUserList(string id, DateTime? beginTime, DateTime? endTime);
    }
}
