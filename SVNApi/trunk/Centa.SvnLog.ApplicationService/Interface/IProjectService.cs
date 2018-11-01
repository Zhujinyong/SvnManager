using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// svn日志服务
    /// </summary>
    public interface IProjectService
    {
        PageDataView<T> GetProjectList<T>(string name,int pageIndex, int pageSize) where T : class;

        void AddProject(SvnProjectModel user);

        void UpdateProject(SvnProjectModel user);

        void DeleteProject(string id);

    }
}
