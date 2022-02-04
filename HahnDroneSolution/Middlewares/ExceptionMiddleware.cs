using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HahnDroneAPI.CustomExceptions;
using HahnDroneAPI.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HahnDroneAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerFactory loggerFactory;

        public ExceptionMiddleware(RequestDelegate _next, ILoggerFactory _loggerFactory)
        {
            this.next = _next;
            this.loggerFactory = _loggerFactory;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                LogExceptionStack(ex);
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var globalErrorDetail = new GlobalErrorDetail();

            if (exception is MessageException)
            {
                context.Response.StatusCode = (int)((MessageException)exception).HttpStatusCode;

                globalErrorDetail.StatusCode = context.Response.StatusCode;
                globalErrorDetail.Message = exception.Message;

            } else
            {
                globalErrorDetail.StatusCode = context.Response.StatusCode;
                globalErrorDetail.Message = "Something went wrong !Internal Server Error";
            }

            var jsonError = JsonConvert.SerializeObject(globalErrorDetail);
            return context.Response.WriteAsync(jsonError);

        }

        private void LogExceptionStack(Exception ex)
        {
            StringBuilder str = new StringBuilder();
            var logger = loggerFactory.CreateLogger<ExceptionMiddleware>();

            while (ex != null)
            {
                str.Append(ex.Message + "\n");
                str.Append("=========================================================================");
                ex = ex.InnerException;
            }

            logger?.LogError($"Something went wrong: { str }");
        }
    }
}
