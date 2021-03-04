using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyIdp.Filter
{
    public class ExceptionsFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionsFilter> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionsFilter(ILogger<ExceptionsFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            //if (context.Exception.GetType().Equals(typeof(UIException)))
            //{
            //    context.Result = new JsonResult(new ReponseModel { Success = false, Data = new List<string>(), Msg = context.Exception.Message });
            //    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            //}
            //else
            //{
            var json = new JsonErrorResponse();

            json.Message = context.Exception.Message;//错误信息
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new InternalServerErrorObjectResult(json);
            //采用log4net 进行错误日志记录
            _logger.LogError(json.Message + WriteLog(json.Message, context.Exception));
            //}
        }
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }
        public class JsonErrorResponse
        {
            /// <summary>
            /// 生产环境的消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 开发环境的消息
            /// </summary>
            public string DevelopmentMessage { get; set; }
        }
    }
}
