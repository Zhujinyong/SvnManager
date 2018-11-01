using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Interfaces;
using Centa.SvnLog.Infrastructure.Model;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class SystemUserService : ISystemUserService
    {
        private readonly IRepository<dynamic> _repository;

        public SystemUserService(IRepository<dynamic> repository)
        {
            _repository = repository;
        }
   
        public PageDataView<T> GetSystemUserList<T>(string realName , string userName, int pageIndex , int pageSize) where T : class
        {
            PageCriteria criteria = new PageCriteria();
            StringBuilder condition = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(realName))
            {
                condition.Append($" and  realname like '%{realName}%'  ");
            }
            if (!string.IsNullOrEmpty(userName))
            {
                condition.Append($" and  UserName like '%{userName}%'  ");
            }
            criteria.Condition = condition.ToString();
            criteria.CurrentPage = pageIndex;
            criteria.Fields = "ID,UserName,Description,IsUse,RealName,Picture";
            criteria.PageSize = pageSize;
            criteria.TableName = $"System_Account a";
            criteria.PrimaryKey = "ID";
            var pageData = _repository.GetPageData<T>(criteria);
            return pageData;
        }

        public void AddUser(SystemUserModel user)
        {
            var query = $"insert into System_Account(ID,UserName,IsDomainAccount,Description,IsUse,RealName,Picture) values('{user.ID}','{user.UserName}',1,'{user.Description}','{user.IsUse}','{user.RealName}','{user.Picture}')";
            _repository.Excute(query);
            _repository.Connection.Close();
        }

        public void UpdateUser(SystemUserModel user)
        {
            var query = $"update System_Account set UserName='{user.UserName}',RealName='{user.RealName}',Description='{user.Description}' ,IsUse='{user.IsUse}',Picture='{user.Picture}'where ID='{user.ID}'";
            _repository.Excute(query);
            _repository.Connection.Close();
        }

    }
}
