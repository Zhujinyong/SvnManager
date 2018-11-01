using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class SvnLogService : ISvnLogService
    {
        private readonly IRepository<dynamic> _repository;

        private readonly string _svnWebServiceUrl;

        private readonly string _jenkinsUrl;

        public SvnLogService(IRepository<dynamic> repository, IOptions<AppSetting> appSetting)
        {
            _repository = repository;
            _svnWebServiceUrl = appSetting.Value.SvnWebServiceUrl;
            _jenkinsUrl= appSetting.Value.JenkinsUrl;
        }

        public PageDataView<SvnLogModel> GetSvnLogList(int pageIndex,int pageSize, string beginTime, string endTime, string projectID, string userID) 
        {
            StringBuilder condition = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(projectID))
            {
                condition.Append($" and l.ProjectID='{projectID}' ");
            }
            if (!string.IsNullOrEmpty(userID))
            {
                condition.Append($" and l.UserID='{userID}' ");
            }
            if (!string.IsNullOrEmpty(beginTime))
            {
                condition.Append($" and l.CreateTime>='{beginTime}' ");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                condition.Append($" and l.CreateTime<='{endTime}' ");
            }
            var list = _repository.GetJoinPageData<SvnLogModel,SvnProjectModel, SvnUserModel, SvnLogModel>(new PageCriteria()
            {
                TableName = @"SVN_Log l inner JOIN SVN_Project p on l.ProjectID = p.ID inner JOIN SVN_User u on l.UserID = u.ID ",
                PrimaryKey = "l.ID",
                Fields = "l.ID,l.CreateTime,l.Revision,l.Message,l.ProjectID,p.Name,l.UserID,u.RealName ",
                Condition = condition.ToString(),
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Sort = "l.CreateTime desc",
            }, (log,project, user) =>
            {
                log.SvnProject = project;
                log.SvnUser = user;
                return log;
            }, "ProjectID,UserID");
            return list;
        }

        public PageDataView<SvnLogFileModel> GetFileList(string logID)
        {
            var list=_repository.GetPageData<SvnLogFileModel>(new PageCriteria()
            {
                TableName = @"SVN_LogFile ",
                PrimaryKey = "ID",
                Fields = "ID,Path,Action ",
                Condition = $" LogID='{logID}'",
                CurrentPage = 1,
                PageSize = 10000
            });
            return list;
        }

        public string GetFileChange(string logFileID)
        {
            var result = string.Empty;
            var list = _repository.Query($"select top 1 l.Revision,p.Url,f.Path ,u.DomainAccount,u.Password from SVN_LogFile as f LEFT JOIN SVN_Log as l on f.LogID = l.ID LEFT JOIN SVN_Project as p on l.ProjectID = p.ID  LEFT JOIN SVN_User as u on p.UserID = u.ID where f.ID = '{logFileID}'").ToList();

            if(list.Count>0)
            {
                var model = list.FirstOrDefault();
                var before = _repository.Query($@"select max(revision) as BeforeRevision from SVN_LogFile as f
LEFT JOIN SVN_Log as l on f.LogID = l.ID
where
cast(f.Path as varchar(max)) = '{model.Path}'
and l.Revision< {model.Revision}").FirstOrDefault();
                using (HttpClient http = new HttpClient())
                {
                    var postData = new List<System.Collections.Generic.KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("userName", model.DomainAccount));
                    postData.Add(new KeyValuePair<string, string>("password", model.Password));
                    postData.Add(new KeyValuePair<string, string>("fileUrl", GetUrl(model.Url ,model.Path)));
                    postData.Add(new KeyValuePair<string, string>("reversion", model.Revision.ToString()));
                    postData.Add(new KeyValuePair<string, string>("beforeReversion", before.BeforeRevision == null ? (model.Revision-1).ToString():before.BeforeRevision.ToString()));
                    HttpContent content = new FormUrlEncodedContent(postData);
                    try
                    {
                        var response = http.PostAsync($"{_svnWebServiceUrl}/SvnService.asmx/GetFileChange", content);
                        result = response.Result.Content.ReadAsStringAsync().Result;
                    }
                    catch(Exception e)
                    {

                    }
                   
                }
            }
            return result;
        }

        private string GetUrl(string prefix,string endfix)
        {
            var result = prefix + endfix;
            var endList=endfix.Split("/").ToList();
            foreach(var item in endList)
            {
                if(string.IsNullOrEmpty(item))
                {
                    continue;
                }
                var temp = $"/{item}";
                if (prefix.Contains(temp))
                {
                    result=prefix.Substring(0, prefix.IndexOf(temp)) + endfix;
                }
                break;
            }
            return result;
        }

        public string GetFileContent(string logFileID)
        {
            var result = string.Empty;
            var list = _repository.Query($"select top 1 l.Revision,p.Url,f.Path ,u.DomainAccount,u.Password from SVN_LogFile as f LEFT JOIN SVN_Log as l on f.LogID = l.ID LEFT JOIN SVN_Project as p on l.ProjectID = p.ID  LEFT JOIN SVN_User as u on p.UserID = u.ID where f.ID = '{logFileID}'").ToList();
            if (list.Count > 0)
            {
                var model = list.FirstOrDefault();
                using (HttpClient http = new HttpClient())
                {
                    var postData = new List<System.Collections.Generic.KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("userName", model.DomainAccount));
                    postData.Add(new KeyValuePair<string, string>("password", model.Password));
                    postData.Add(new KeyValuePair<string, string>("fileUrl", GetUrl(model.Url , model.Path)));
                    postData.Add(new KeyValuePair<string, string>("reversion", model.Revision.ToString()));
                    HttpContent content = new FormUrlEncodedContent(postData);
                    try
                    {
                        var response = http.PostAsync($"{_svnWebServiceUrl}/SvnService.asmx/GetFileContent", content);
                        result = response.Result.Content.ReadAsStringAsync().Result;
                    }
                    catch(Exception e)
                    {

                    }
                    
                }
            }
            return result;
        }

        public bool PublishRevision(string logID)
        {
            var jobName = string.Empty;
            using (HttpClient http = new HttpClient())
            {
                try
                {
                    var response = http.PostAsync($"{_jenkinsUrl}/job/jobName/build", null);
                   var  result = response.Result.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
