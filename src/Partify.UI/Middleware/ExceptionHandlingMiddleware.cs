namespace CSOS.UI.Middleware;

/// <summary>
/// Middleware that intercepts exceptions thrown during HTTP request processing and handles them appropriately.
/// </summary>
/// <remarks>This middleware should be registered early in the ASP.NET Core request pipeline to ensure that
/// unhandled exceptions are logged and, in the case of request cancellations, an appropriate status code is returned.
/// For canceled requests, the response status code is set to 499 and no further processing occurs. For other
/// exceptions, the error is logged and rethrown, allowing higher-level middleware or the framework to handle it. This
/// middleware is typically used to provide consistent error logging and response behavior across the
/// application.</remarks>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {

            if (ex is TaskCanceledException or OperationCanceledException)
            {
                httpContext.Response.StatusCode = 499;
                _logger.LogWarning("Operation Canceled - user closed request");
                return;
            }
            _logger.LogError(ex, "Unhandled error occured");

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = 500;
            throw;
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
