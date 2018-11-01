
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.ApplicationService.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="domainAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        int Login(string domainAccount, string password, StateEnum IsDomainAccount);

        AccountModel GetAccount(string userName);
    }
}
