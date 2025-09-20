using System.Text.Json;
using Shared;
using Shared.Exceptions;

namespace Web.MiddleWares;

public class ExceptionMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleWare> _logger;

    public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        (int code, Error[]? errors) = exception switch
        {
            BadRequestException =>
                (StatusCodes.Status500InternalServerError, JsonSerializer.Deserialize<Error[]>(exception.Message)),

            NotFoundException =>
                (StatusCodes.Status404NotFound, JsonSerializer.Deserialize<Error[]>(exception.Message)),

            _ =>
                (StatusCodes.Status500InternalServerError, [Error.Failure(null, "Произошла внутрення ошибка сервера")])
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        
        await context.Response.WriteAsJsonAsync(errors);
    }
    
}

public static class ExceptionMiddleWareExtension
{
    public static IApplicationBuilder UseExceptionMiddleWare(this WebApplication app) =>
        app.UseMiddleware<ExceptionMiddleWare>();
}