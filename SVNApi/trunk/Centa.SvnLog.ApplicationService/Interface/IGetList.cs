using Centa.SvnLog.Infrastructure.General.Page;

namespace Centa.SvnLog.ApplicationService.Interface
{
    public interface IGetList
    {
        PageDataView<T> GetList<T>(string companyCode, string viewName, string primaryKey, int pageIndex, int pageSize,  string dateField, string modStartDate, string modEndDate, string where) where T : class;
    }
}