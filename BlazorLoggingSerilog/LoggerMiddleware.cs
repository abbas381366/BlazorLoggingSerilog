using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;
using UAParser;

namespace BlazorLoggingSeriLog
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string uaSTR = httpContext.Request.Headers["User-Agent"];
            if (string.IsNullOrWhiteSpace(uaSTR) == false)
            {
                var parser = Parser.GetDefault();

                UserAgent userAgent = parser.ParseUserAgent(uaSTR);
                OS os = parser.ParseOS(uaSTR);
                Device device = parser.ParseDevice(uaSTR);
                LogContext.PushProperty("Browser", userAgent.Family);
                LogContext.PushProperty("Browser version", userAgent.Major);
                LogContext.PushProperty("OS", os.Family);
                LogContext.PushProperty("OS Version", os.Major);
                LogContext.PushProperty("Device Brand", device.Brand);
                LogContext.PushProperty("Device Model", device.Model);
            }
            LogContext.PushProperty("User Name", httpContext.User.Identity?.Name);
            LogContext.PushProperty("Channel", httpContext.User.Claims.FirstOrDefault(q => q.Type == "Channel")?.Value);
            LogContext.PushProperty("IP", httpContext.Connection.RemoteIpAddress?.ToString());
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }
}
