using Hozify.Application.Common;
using Hozify.Domain.Constants;
using Hozify.Domain.Enums;

namespace Hozify.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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

        // Business Exceptions
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ApiStatusCode.BadRequest;

            var response = ResponseFactory.Failure<object>(
                ex.Message,
                ApiStatusCode.BadRequest);

            await context.Response.WriteAsJsonAsync(response);
        }

        // Unexpected Exceptions
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ApiStatusCode.InternalServerError;

            var response = ResponseFactory.Failure<object>(
                ApiMessages.InternalServerError,
                ApiStatusCode.InternalServerError);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}