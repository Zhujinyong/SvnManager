using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.ModelBinder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Centa.SvnLog.WebApi.Swagger
{
    /// <summary>
    /// swagger请求参数设置
    /// 模型绑定的TokenModel参数，swagger忽略掉，用token参数取而代之
    /// 参数排序（如果没有继承，只需要改变propertity顺序，但是有继承，就要设置order了）
    /// </summary>
    public class ParameterFilter : IOperationFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            #region header里增加token参数
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            //查找模型绑定的TokenModel参数
            var isExist = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is TokenValidateAttribute);
            //存在，用token代替
            if (isExist)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                context.ApiDescription.ParameterDescriptions
                    .Where(desc => desc.ParameterDescriptor != null && desc.ParameterDescriptor.ParameterType != null && desc.ParameterDescriptor.ParameterType == typeof(TokenModel))
                    .ToList()
                    .ForEach(param =>
                    {
                        var toRemove = operation.Parameters
                          .FirstOrDefault(p => p.Name == param.Name);
                        //删除
                        if (null != toRemove)
                            operation.Parameters.Remove(toRemove);
                    });
                //添加
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "token",
                    In = "header",
                    Description = "@Order=2,access token",
                    Required = true,
                    Type = "string",
                });
            }
            #endregion
            #region 参数排序
            var parames = operation.Parameters.OrderBy(p => p, new ParameterSortComparer()).ToList();
            foreach (var parameter in parames)
            {
                SetDescription(parameter);
            }
            operation.Parameters.Clear();
            operation.Parameters = parames;
            #endregion
        }

        /// <summary>
        /// 修改字段描述
        /// </summary>
        /// <param name="parameter"></param>
        private void SetDescription(IParameter parameter)
        {
            var startString = @"Order=";
            var endString = @",";
            if (!string.IsNullOrEmpty(parameter.Description) && parameter.Description.Contains(startString) && parameter.Description.Contains(endString))
            {
                parameter.Description = parameter.Description.Substring(parameter.Description.IndexOf(endString) + 1);
            }
        }

        /// <summary>
        /// 参数排序，参数格式：Order={序号},
        /// </summary>
        class ParameterSortComparer : IComparer<IParameter>
        {
            public int Compare(IParameter p1, IParameter p2)
            {
                return GetSort(p1) - GetSort(p2);
            }

            /// <summary>
            /// 从注释里获取排序
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            private int GetSort(IParameter parameter)
            {
                var startString = @"Order=";
                var endString = @",";
                var sort = 0;
                if (!string.IsNullOrEmpty(parameter.Description) && parameter.Description.Contains(startString) && parameter.Description.Contains(endString))
                {
                    var sortString = parameter.Description.Substring(parameter.Description.IndexOf(startString) + startString.Length, parameter.Description.IndexOf(endString) - startString.Length - parameter.Description.IndexOf(startString));
                    int.TryParse(sortString, out sort);
                }
                return sort;
            }
        }
    }
}