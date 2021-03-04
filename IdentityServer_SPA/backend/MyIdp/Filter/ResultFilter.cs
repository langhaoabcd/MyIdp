
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIdp.Filter
{
    public class ResultFilter : ActionFilterAttribute
    {
        JsonSerializerSettings serializeOptions = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };
        public ResultFilter()
        {
            
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {

            base.OnResultExecuted(context);
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            {
                context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
            // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
            //csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            // once for standards compliant browsers
            if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
            }
            // and once again for IE
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
            {
                context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            var referrer_policy = "no-referrer";
            if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
            {
                context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
            }
            if (context.ModelState.IsValid)
            {
                var res = context.Result.GetType();
                if (!res.Equals(typeof(EmptyResult)) && !res.Equals(typeof(SignOutResult)) && !res.Equals(typeof(RedirectResult)))
                {
                    var value = (context.Result as dynamic)?.Value;
                    Console.WriteLine($"\n输出:{value}\n");
                    //var res = JsonSerializer.Serialize(new ReponseModel { Success = true, Data = value, Msg = "" });
                    
                    context.Result = new JsonResult(new ReponseModel { Success = true, Data = value, Msg = "" });

                    context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                }
                base.OnResultExecuting(context);
            }
            else
            {
                var list = new List<string>();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        list.Add(error.ErrorMessage);
                    }
                }
                context.Result = new JsonResult(new ReponseModel { Success = false, Data = list, Msg = "参数错误" });
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            await base.OnResultExecutionAsync(context, next);
        }
        //public override void OnResultExecuting(ResultExecutingContext context)
        //{

        //}
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }

    public class ReponseModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "Success";
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
