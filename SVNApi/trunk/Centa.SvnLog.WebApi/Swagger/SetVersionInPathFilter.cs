using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Centa.SvnLog.WebApi.Swagger
{
    /// <summary>
    /// 路径上自动设置版本
    /// </summary>
    public class SetVersionInPathFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(
                    path => path.Key.Replace("{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }
    }
}