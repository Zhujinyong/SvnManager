using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.ApplicationService
{
    /// <summary>
    /// 日志
    /// </summary>
    public class SystemLogService : ISystemLogService
    {
        private readonly IRepository<SystemLogModel> _repository;

        public SystemLogService(IRepository<SystemLogModel> repository)
        {
            _repository = repository;
        }

        public long Add(SystemLogModel log)
        {
            return _repository.Add(log);
        }

        public int UpdateExcuteTime(string id, long elapsedMilliseconds)
        {
            return _repository.Excute($"update systemlog set ExcuteMilliseconds={elapsedMilliseconds} where id='{id}'");
        }

        public PageDataView<SystemLogModel> GetList(string url, string companyName, LogTypeEnum? logType, int pageIndex = 1, int pageSize = 10)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.Condition = " 1=1 ";
            if (!string.IsNullOrEmpty(companyName))
            {
                criteria.Condition += $" and companyName like '%{companyName}%' ";
            }
            if (logType.HasValue)
            {
                criteria.Condition += $" and logType ={logType} ";
            }
            criteria.CurrentPage = pageIndex;
            criteria.Fields = "*";
            criteria.PageSize = pageSize;
            criteria.TableName = "SystemLog a";
            criteria.PrimaryKey = "CreateTime";
            var pageData = _repository.GetPageData<SystemLogModel>(criteria);
            return pageData;
        }
    }
}
