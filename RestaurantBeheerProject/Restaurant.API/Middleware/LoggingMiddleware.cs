using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class LoggingMiddleware {
    private RequestDelegate _next;
    private ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next ,ILoggerFactory loggerFactory) {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } finally {
            _logger.LogInformation("Request {method} {url} => {statuscode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }
    }
}
