using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoHosting.Core.Middlewares
{
    public class LogMiddleware
    {
        RequestDelegate _next;
        ILogger<LogMiddleware> logger;
        public LogMiddleware(RequestDelegate requestDelegate, ILogger<LogMiddleware> logger)
        {
            _next = requestDelegate;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(VideoHosting.Core.Exceptions.InvalidDataException ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    logger.LogError(ex.Message, ex);
                }
                else
                {
                    logger.LogError("Error InvalidData", ex);
                }
                context.Response.StatusCode = 400;
            }
            catch(UnauthorizedAccessException ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    logger.LogError(ex.Message, ex);
                }
                else
                {
                    logger.LogError("Error Unauthorized", ex);
                }
                context.Response.StatusCode = 403;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.Body.Write(bytes);
                    logger.LogCritical(ex.Message, ex);
                }
                else
                {
                    logger.LogCritical("Fatal",ex);
                }
                context.Response.StatusCode = 501;
                
            }
        }
    }
}
