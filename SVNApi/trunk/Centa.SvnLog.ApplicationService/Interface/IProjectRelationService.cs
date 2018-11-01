using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// svn日志服务
    /// </summary>
    public interface IProjectRelationService
    {

        void AddProjectRelation(SvnProjectRelationModel user);

        void UpdateProjectRelation(SvnProjectRelationModel user);

        void DeleteProjectRelation(string id);

        List<dynamic> GetProjectTreeList(string id);

        List<dynamic> GetTreeList(string id);
    }
}
