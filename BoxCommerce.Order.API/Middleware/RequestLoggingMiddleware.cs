using BoxCommerce.Orders.API.Middleware;

namespace API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _hostingEnv;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostingEnv)
    {
        _next = next;
        _logger = logger;
        _hostingEnv = hostingEnv;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var valuesDictionary = new Dictionary<string, object>();

        if(httpContext?.Connection?.RemoteIpAddress != null)
        {
            valuesDictionary.Add("IpAddress", httpContext.Connection.RemoteIpAddress.ToString());
        }

        if (httpContext.Request.Headers.ContainsKey("x-correlation-id"))
        {
            var value = httpContext.Request.Headers["x-correlation-id"];
            valuesDictionary.Add("x-correlation-id", value);
        } else
        {
            var value = Guid.NewGuid();
            valuesDictionary.Add("x-correlation-id", value);
        }

        var userAgentExists = httpContext?.Request?.Headers?.TryGetValue("User-Agent", out var userAgent);

        if (httpContext.Request.Headers.TryGetValue("User-Agent", out var headerValues))
        {
            // Return the first header value
            valuesDictionary.Add("UserAgent", headerValues.ToString());
        }

        using (_logger.BeginScope(valuesDictionary))
        {
            await _next(httpContext);
        }
    }

}
