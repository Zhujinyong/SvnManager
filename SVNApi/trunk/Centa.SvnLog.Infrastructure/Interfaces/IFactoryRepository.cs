
namespace Centa.SvnLog.Infrastructure.Interfaces
{
    /// <summary>
    /// 创建仓库接口
    /// </summary>
    public interface IFactoryRepository
    {
        /// <summary>
        /// 创建仓库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        IRepository<T> CreateRepository<T>(IDapperContext context) where T : class;
    }
}