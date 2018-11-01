using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Centa.SvnLog.Infrastructure;
using Centa.SvnLog.WebApi.Filter.Action;
using Centa.SvnLog.WebApi.Filter.Exception;
using Centa.SvnLog.WebApi.ModelBinder;
using Centa.SvnLog.WebApi.Token;
using Centa.SvnLog.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Common.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using System;

namespace Centa.SvnLog.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            #region gzip压缩
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[] {
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    "image/svg+xml"
                };
            });
            #endregion
            //注入AppSetting，其他类库可以引用，如DapperContext构造函数用到了
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion
            #region  依赖注入相关自定义接口
            services.AddSingleton<ITokenVerify, TokenVerify>();
            services.RegisterRepository("Centa.SvnLog.Infrastructure", string.Empty);
            services.RegisterService("Centa.SvnLog.ApplicationService", string.Empty);
            #endregion
            services.AddRouting(options => options.LowercaseUrls = true);
            #region swagger
            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider()
                        .GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {

                        options.SwaggerDoc(
                            description.GroupName,
                            new Info()
                            {
                                Title = $"{Configuration.GetSection("AppSetting:ApiTitle").Value}",
                                Version = description.ApiVersion.ToString()
                            });
                    }
                    options.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        var versions = apiDesc.ControllerAttributes()
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);
                        return versions.Any(v => $"v{v.ToString()}" == docName);
                    });
                    options.DescribeAllEnumsAsStrings();
                    //模型绑定TokenModel参数设置
                    options.OperationFilter<FileUploadFilter>();
                    options.OperationFilter<ParameterFilter>();

                    options.DocumentFilter<SetVersionInPathFilter>();
                    var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
                    Console.WriteLine(basePath);
                    options.IncludeXmlComments(System.IO.Path.Combine(basePath, $"Centa.SvnLog.WebApi.xml"));
                    //Dto作为一个单独项目，需要也包含就来，否则生成的xml里没有相应的注释
                    options.IncludeXmlComments(System.IO.Path.Combine(basePath, $"Centa.SvnLog.Dto.xml"));
                });
            #endregion
            #region apiVerison
            services.AddApiVersioning(config =>
             {
                 config.ReportApiVersions = true;
                 config.AssumeDefaultVersionWhenUnspecified = true;
                 config.DefaultApiVersion = new ApiVersion(1, 0);
             });
            #endregion
            #region mvc
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SystemLogAttribute));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                options.ModelBinderProviders.Insert(0, new TokenModelBinderProvider(services.BuildServiceProvider().GetService<ITokenVerify>(), services.BuildServiceProvider().GetService<IAccountService>()));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            #endregion
            #region Mini
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            #region swagger configure
            app.UseSwagger(c => { });
            app.UseSwaggerUI(
                   options =>
                   {
                       options.DefaultModelRendering(ModelRendering.Model);
                       options.DefaultModelsExpandDepth(-1);
                       options.DisplayRequestDuration();
                       options.DocumentTitle = $"{Configuration.GetSection("AppSetting:ApiTitle").Value}";
                       foreach (var description in provider.ApiVersionDescriptions)
                       {
                           options.SwaggerEndpoint(
                               $"/swagger/{description.GroupName}/swagger.json",
                               description.GroupName.ToUpperInvariant());
                       }
                   });
            #endregion
            app.UseCors("AllowAllOrigins");
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
