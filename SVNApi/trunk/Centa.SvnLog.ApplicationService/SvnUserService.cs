using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class SvnUserService : ISvnUserService
    {
        private readonly IRepository<dynamic> _repository;

        public SvnUserService(IRepository<dynamic> repository)
        {
            _repository = repository;
        }
   
        public PageDataView<T> GetSvnUserList<T>(string realName , string domainAccount, int pageIndex , int pageSize) where T : class
        {
            PageCriteria criteria = new PageCriteria();
            StringBuilder condition = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(realName))
            {
                condition.Append($" and  realname like '%{realName}%'  ");
            }
            if (!string.IsNullOrEmpty(domainAccount))
            {
                condition.Append($" and domainAccount like '%{domainAccount}%'  ");
            }
            criteria.Condition = condition.ToString();
            criteria.CurrentPage = pageIndex;
            criteria.Fields = "ID,RealName,DomainAccount,IsUse,Description";
            criteria.PageSize = pageSize;
            criteria.TableName = $"SVN_User a";
            criteria.PrimaryKey = "ID";
            var pageData = _repository.GetPageData<T>(criteria);
            return pageData;
        }

        public void AddSvnUser(SvnUserModel user)
        {
            var query = $"insert into SVN_User(ID,DomainAccount,RealName,IsUse,Description) values('{user.ID}','{user.DomainAccount}','{user.RealName}',{user.IsUse},'{user.Description}')";
            _repository.Excute(query);
        }

        public void UpdateSvnUser(SvnUserModel user)
        {
            var query = $"update SVN_User set DomainAccount='{user.DomainAccount}',RealName='{user.RealName}',IsUse='{user.IsUse}',Description='{user.Description}' where ID='{user.ID}'";
             _repository.Excute(query);
        }

    }
}
