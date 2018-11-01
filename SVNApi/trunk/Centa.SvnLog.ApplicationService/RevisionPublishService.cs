using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Common;
using Centa.SvnLog.Infrastructure;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Centa.SvnLog.ApplicationService
{
    public class RevisionPublishService : IRevisionPublishService
    {
        private readonly IRepository<dynamic> _repository;

        private readonly string _jenkinsUrl;

        public RevisionPublishService(IRepository<dynamic> repository, IOptions<AppSetting> appSetting)
        {
            _repository = repository;
            _jenkinsUrl = appSetting.Value.JenkinsUrl;
        }

        public List<dynamic> GetRevisionPublishList(string logID)
        {
            var list = _repository.Query($"select p.ID,p.LogID,p.JenkinsID,p.CreateTime,p.Description ,j.Name,j.JobName,a.RealName from SVN_RevisionPublish as p LEFT JOIN Svn_Jenkins as j on p.JenkinsID = j.ID LEFT JOIN System_Account as a on p.SystemAccountID=a.ID where p.LogID='{logID}' ORDER BY p.CreateTime desc");
            return list.ToList();
        }

        public ExcuteMessage AddRevisionPublish(SvnRevisionPublishModel model)
        {
            var message = new ExcuteMessage();
            var jenkinsOptions=_repository.Query($"select JobName from Svn_Jenkins where ID='{model.JenkinsID}'").FirstOrDefault();
            if (jenkinsOptions == null)
            {
                message.Success = false;
                message.Message = "不存在Job";
            }
            else
            {
                using (HttpClient http = new HttpClient())
                {
                    try
                    {
                        var response = http.PostAsync($"{_jenkinsUrl}/job/{jenkinsOptions.JobName}/build", null);
                        var result = response.Result.Content.ReadAsStringAsync().Result;
                        var query = $"insert into SVN_RevisionPublish(SystemAccountID,LogID,JenkinsID,Description) values('{model.SystemAccountID}','{model.LogID}','{model.JenkinsID}','{model.Description}')";
                        _repository.Excute(query);
                    }
                    catch (Exception e)
                    {
                        message.Success = false;
                        message.Message = e.ToString();
                    }
                }
            }
            return message;
        }
    }
}
