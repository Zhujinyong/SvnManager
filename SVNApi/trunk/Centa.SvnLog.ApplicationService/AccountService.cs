using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Infrastructure.Interfaces;
using System.Linq;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using Centa.SvnLog.Infrastructure.Model.Enum;

namespace Centa.SvnLog.ApplicationService
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IRepository<AccountModel> _repository;

        public AccountService(IRepository<AccountModel> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainAccount"></param>
        /// <param name="password"></param>
        /// <returns>-1不存在,-2域验证失败，1成功</returns>
        public int Login(string domainAccount, string password, StateEnum isDomainAccount)
        {

            int state = 0;
            #region 域验证
            if (isDomainAccount== StateEnum.Yes)
            {
                string domainName = "centaline.com.cn";
                string userDn = $"{domainAccount}@{domainName}";
                try
                {
                    using (var connection = new LdapConnection { SecureSocketLayer = false })
                    {
                        connection.Connect(domainName, LdapConnection.DEFAULT_PORT);
                        connection.Bind(userDn, password);
                        if (connection.Bound)
                        {
                            state = 1;
                        }
                    }
                }
                catch (LdapException ex)
                {
                    state = -2;
                }
                
            }
            #endregion
            #region 系统账号
            else
            {
                var model = _repository.Query($"select 1 from System_Account where  UserName='{domainAccount}' and Password='{password}' and  IsUse=1");
                state = model.Count() > 0 ? 1 : -1;
            }
            #endregion
            return state;
        }

        public AccountModel GetAccount(string userName) => _repository.GetBy($"select * from System_Account where  userName='{userName}' and  IsUse=1 ").FirstOrDefault();
    }
}
