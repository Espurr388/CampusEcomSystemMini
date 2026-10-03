using CampusEcomSystemMini.Application.Exceptions;

namespace CampusEcomSystemMini.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException exception)
        {
            _logger.LogWarning(
                exception,
                "Business rule error on {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            var statusCode = exception.ErrorType switch
            {
                AppErrorType.Validation =>
                    StatusCodes.Status400BadRequest,

                AppErrorType.NotFound =>
                    StatusCodes.Status404NotFound,

                AppErrorType.Conflict =>
                    StatusCodes.Status409Conflict,

                AppErrorType.Forbidden =>
                    StatusCodes.Status403Forbidden,

                _ => StatusCodes.Status400BadRequest
            };

            context.Response.Clear();

            context.Response.StatusCode = statusCode;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new { message = exception.Message });
        }
    }
}