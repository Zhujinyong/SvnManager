using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Centa.SvnLog.Common.DependencyInjection
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 注册业务服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="interfaceAssemblyName">定义业务接口的程序集名称</param>
        /// <param name="implementAssemblyName">实现业务接口的程序集名称(默认 interfaceAssemblyName)</param>
        public static void RegisterService(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (string.IsNullOrEmpty(implementAssemblyName))
            {
                RegisterAssembly(service, interfaceAssemblyName);
            }
            else
            {
                RegisterAssembly(service, interfaceAssemblyName, implementAssemblyName);
            }
        }

        /// <summary>
        /// 注册数据库相关
        /// </summary>
        /// <param name="services"></param>
        /// <param name="implementAssemblyName"></param>
        public static void RegisterRepository(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (string.IsNullOrEmpty(implementAssemblyName))
            {
                RegisterAssembly(service, interfaceAssemblyName);
            }
            else
            {
                RegisterAssembly(service, interfaceAssemblyName, implementAssemblyName);
            }
        }

        /// <summary>
        /// 批量注入接口程序集中对应的实现类。
        /// <para>
        /// 需要注意的是，这里有如下约定：
        /// IUserService --> UserService, IUserRepository --> UserRepository.
        /// </para>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        internal static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName)
        {
            if (service == null)
            { 
                throw new ArgumentNullException(nameof(service));
            }
            if (string.IsNullOrEmpty(interfaceAssemblyName))
            {
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            }
            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }
            //过滤掉非接口及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface);
            foreach (var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                    service.AddScoped(type, implementType);
            }
            return service;
        }

        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <param name="implementAssemblyName">实现程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        internal static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            if (string.IsNullOrEmpty(implementAssemblyName))
                throw new ArgumentNullException(nameof(implementAssemblyName));
            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (interfaceAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }
            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            if (implementAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{implementAssemblyName}\" not be found");
            }
            //过滤掉非接口及泛型接口
            var types = interfaceAssembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface);
            foreach (var type in types)
            {
                //过滤掉抽象类、泛型类以及非class
                var implementType = implementAssembly.DefinedTypes
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract &&
                                         t.GetInterfaces().Any(b => b.Name == type.Name));
                if (implementType != null)
                {
                    service.AddScoped(type, implementType.AsType());
                }
            }
            return service;
        }
    }
}