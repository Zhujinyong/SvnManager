using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class SvnJenkinsService : ISvnJenkinsService
    {
        private readonly IRepository<dynamic> _repository;

        public SvnJenkinsService(IRepository<dynamic> repository)
        {
            _repository = repository;
        }
   
        public List<dynamic> GetSvnJenkinsList(string projectRelationID)
        {
            StringBuilder sb = new StringBuilder("select ID,ProjectRelationID,Name,JobName,Description from SVN_Jenkins");
            if(!string.IsNullOrEmpty(projectRelationID))
            {
                sb.Append($" where ProjectRelationID='{projectRelationID}'");
            }
            var list= _repository.Query(sb.ToString());
            return list.ToList();
        }

        public void AddSvnJenkins(SvnJenkinsModel jenkins)
        {
            var query = $"insert into SVN_Jenkins(ProjectRelationID,Name,JobName,Description) values('{jenkins.ProjectRelationID}','{jenkins.Name}','{jenkins.JobName}','{jenkins.Description}')";
            _repository.Excute(query);
        }

        public void UpdateSvnJenkins(SvnJenkinsModel jenkins)
        {
            var query = $"update SVN_Jenkins set Name='{jenkins.Name}',JobName='{jenkins.JobName}',Description='{jenkins.Description}' where ID='{jenkins.ID}'";
             _repository.Excute(query);
        }

    }
}
