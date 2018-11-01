using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public interface ISystemUserService
    {
        PageDataView<T> GetSystemUserList<T>(string realName ,string userName, int pageIndex, int pageSize) where T : class;

        void AddUser(SystemUserModel user);

        void UpdateUser(SystemUserModel user);
    }
}
