using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<dynamic> _repository;

        private IUnitOfWork _unitOfWork;

        public ProjectService(IRepository<dynamic> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public PageDataView<T> GetList<T>( string tableName, string primaryKey, int pageIndex = 1, int pageSize = 10, string where = null) where T : class
        {
            PageCriteria criteria = new PageCriteria();
            StringBuilder condition = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(where))
            {
                condition.Append($"  {where}  ");
            }
            criteria.Condition = condition.ToString();
            criteria.CurrentPage = pageIndex;
            criteria.Fields = "*";
            criteria.PageSize = pageSize;
            criteria.TableName = $"{tableName} a";
            criteria.PrimaryKey = primaryKey;
            var pageData = _repository.GetPageData<T>(criteria);
            return pageData;
        }

        public PageDataView<T> GetProjectList<T>(string name,int pageIndex, int pageSize) where T : class
        {
            PageCriteria criteria = new PageCriteria();
            StringBuilder condition = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(name))
            {
                condition.Append($" and  name like '%{name}%'  ");
            }
            criteria.Condition = condition.ToString();
            criteria.CurrentPage = pageIndex;
            criteria.Fields = "p.ID,p.Name,p.Type,p.Url,p.Description,p.head,p.CreateTime,p.City,p.IsUse,p.Creator";
            criteria.PageSize = pageSize;
            criteria.TableName = $"SVN_Project as p ";
            criteria.PrimaryKey = "p.ID";
            return  _repository.GetPageData<T>(criteria);

        }

        public void AddProject(SvnProjectModel project)
        {
            var query = $"insert into SVN_Project(ID,Name,Url,Type,Description,Head,City,IsUse,Creator) values('{project.ID}','{project.Name}','{project.Url}','{project.Type}','{project.Description}','{project.Head}','{project.City}',{project.IsUse},'{project.Creator}')";
            _repository.Excute(query);
            _repository.Connection.Close();
        }


        public void DeleteProject(string id)
        {
            //删除日志，文件，工程
            var query = $@"
delete from SVN_LogFile 
where LogID in (
select ID
from SVN_Log 
where ProjectID='{id}'
);
delete
from SVN_Log 
where ProjectID='{id}';
delete from  SVN_Project where id='{id}'";
            _unitOfWork.RunTransaction(transcation =>
            {
                _unitOfWork.Excute(query);
            });
        }


        public void UpdateProject(SvnProjectModel project)
        {
            var query = $"update SVN_Project set Name='{project.Name}',Url='{project.Url}',Type='{project.Type}' ,Description='{project.Description}',Head='{project.Head}' ,City='{project.City}',IsUse={project.IsUse},Creator='{project.Creator}' where ID='{project.ID}'";
            _repository.Excute(query);
            _repository.Connection.Close();
        }

    
    }
}
