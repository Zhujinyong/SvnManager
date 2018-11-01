using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class ProjectRelationService : IProjectRelationService
    {
        private readonly IRepository<dynamic> _repository;

        public ProjectRelationService(IRepository<dynamic> repository)
        {
            _repository = repository;
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

      
        public void AddProjectRelation(SvnProjectRelationModel projectRelation)
        {
            var query = $"insert into SVN_ProjectRelation(ID,ChildID,ParentID) values('{projectRelation.ID}','{projectRelation.ChildID}','{projectRelation.ParentID}')";
            if(string.IsNullOrEmpty(projectRelation.ParentID))
            {
                query = $"insert into SVN_ProjectRelation(ID,ChildID) values('{projectRelation.ID}','{projectRelation.ChildID}')";

            }
            _repository.Excute(query);
        }

        public void DeleteProjectRelation(string id)
        {
            var query = $"delete from  SVN_ProjectRelation where id='{id}'";
           
            _repository.Excute(query);
        }

        public void UpdateProjectRelation(SvnProjectRelationModel projectRelation)
        {
            var query = $"update SVN_ProjectRelation set ChildID='{projectRelation.ChildID}',ParentID='{projectRelation.ParentID}' where ID='{projectRelation.ID}'";
            _repository.Excute(query);
        }

        public List<dynamic> GetProjectTreeList(string id)
        {
            StringBuilder sb = new StringBuilder(@"select r.ID as ID,p.Name
                                             from SVN_ProjectRelation as r
                                             LEFT JOIN SVN_Project as p on r.ChildID = p.ID ");
            if(string.IsNullOrEmpty(id)|| id=="root")
            {
                sb.Append("where r.ParentID is null or r.ParentID=''");
            }
            else
            {
                sb.Append($"where r.ParentID='{id}'");
            }
            var list = _repository.Query(sb.ToString());
            return list.ToList();
        }

        public List<dynamic> GetTreeList(string id)
        {
            StringBuilder sb = new StringBuilder($@"
with tree as(
select ChildID,ID,ParentID from SVN_ProjectRelation    where ParentID IS NULL OR ParentID=''
union all
select a.ChildID,a.ID,a.ParentID from SVN_ProjectRelation a  join tree b on a.ParentID=b.id
)select tree.ID , tree.ChildID ,tree.ParentID, p.Name from tree  LEFT JOIN SVN_Project p on ChildID=p.ID");
            
                var sourceList = _repository.Query(sb.ToString()).ToList();
           // List<ProjectTree> sourceList=data as List<ProjectTree>;
            List<dynamic> list;
            FillRecursively(ref sourceList, out list, id);
            return list;
        }

        private void FillRecursively(ref List<dynamic> source, out List<dynamic> outValue, string currentID)
        {
            var matches =string.IsNullOrEmpty(currentID)?source.Where(r => string.IsNullOrEmpty(r.ParentID)).ToArray(): source.Where(r => r.ParentID == currentID).ToArray();
            source.RemoveAll(r => r.ParentID == currentID);
            outValue = new List<dynamic>();

            foreach (var row in matches)
            {
                dynamic project = new ExpandoObject();
                project.Label = row.Name;
                project.ID = row.ID;
                var newChildren = new List<dynamic>();
                FillRecursively(ref source, out newChildren, project.ID);
                if(newChildren.Count>0)
                {
                    project.Children = newChildren;
                }
                outValue.Add(project);
            }
        }
    }
}
