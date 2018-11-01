using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.WebApi.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Diagnostics;
using Centa.SvnLog.Infrastructure.Model;
using Centa.SvnLog.Infrastructure.Model.Enum;
using Centa.SvnLog.Infrastructure.General.Exception;

namespace Centa.SvnLog.WebApi.Filter.Action
{
    /// <summary>
    /// 日志属性，可以加在Controller和Action上
    /// </summary>
    public class SystemLogAttribute : TypeFilterAttribute
    {
        public SystemLogAttribute() : base(typeof(SystemLogActionFilterImpl))
        {
        }

        private class SystemLogActionFilterImpl : IActionFilter
        {
            private readonly ISystemLogService _logService;

            private ActionExecutingContext _acitonContext;

            private SystemLogModel _log;

            private Stopwatch _stopWatch;

            private bool HasToken
            {
                get
                {
                    return _acitonContext.ActionArguments.ContainsKey("tokenModel") && _acitonContext.ActionArguments["tokenModel"].GetType().ToString().Contains("TokenModel");
                }
            }

            private void SetLogProperty()
            {
                if (HasToken)
                {
                    var tokenModel = _acitonContext.ActionArguments["tokenModel"] as TokenModel;
                    _log.UserName = tokenModel.UserName;
                }
            }

            public SystemLogActionFilterImpl(ILoggerFactory loggerFactory, ISystemLogService logService)
            {
                _logService = logService;
            }

            /// <summary>
            /// excute before action run 
            /// </summary>
            /// <param name="context"></param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                _stopWatch = new Stopwatch();
                _stopWatch.Start();
                _acitonContext = context;
                var request = context.HttpContext.Request;
                _log = new SystemLogModel()
                {
                    ID = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    UserIP = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Url = request.Path,
                    Method = request.Method,
                    LogType = LogTypeEnum.Log
                };
                SetLogProperty();
                switch (request.Method)
                {
                    case "GET":
                        _log.Params = request.QueryString.ToString();
                        break;
                    case "POST":
                        var stream = context.HttpContext.Request.Body;
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            _log.Params = reader.ReadToEnd().Trim();
                        }
                        break;
                    case "PUT":
                        break;
                    case "DELETE":
                        break;
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _stopWatch.Stop();
                _log.ExcuteMilliseconds = _stopWatch.ElapsedMilliseconds;
                if (context.Exception!=null&&context.Exception.GetType() != typeof(CustomException))
                {
                    _log.LogType = LogTypeEnum.Error;
                    _log.ErrorMsg = context.Exception.ToString();
                }
                _logService.Add(_log);
            }
        }
    }
}
