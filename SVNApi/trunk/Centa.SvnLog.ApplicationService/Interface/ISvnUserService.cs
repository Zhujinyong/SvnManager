using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// svn用户服务
    /// </summary>
    public interface ISvnUserService
    {
        PageDataView<T> GetSvnUserList<T>(string realName,string domainAccount,int pageIndex, int pageSize) where T : class;

        void AddSvnUser(SvnUserModel user);

        void UpdateSvnUser(SvnUserModel user);
    }
}
