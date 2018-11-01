using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISvnJenkinsService
    {
        List<dynamic> GetSvnJenkinsList(string projectRelationID);

        void AddSvnJenkins(SvnJenkinsModel user);

        void UpdateSvnJenkins(SvnJenkinsModel user);
    }
}
