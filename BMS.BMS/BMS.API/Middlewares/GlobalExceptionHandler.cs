using System.Text.Json;
using BMS.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BMS.API.Middlewares;

// new handling for dotnet 8
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred.");

        httpContext.Response.ContentType = "application/json";

        // Check if the exception is a custom exception (inherited from Exception)
        if (exception is CustomException customException)
        {
            httpContext.Response.StatusCode = customException.StatusCode;
            
            var errorResponse = new
            {
                error = exception.Message
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await httpContext.Response.WriteAsync(result, cancellationToken);

            return true;
        }

        // Handle other types of exceptions
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var genericErrorResponse = new
        {
            error = "An unexpected error occurred. Please try again later."
        };

        var genericResult = JsonSerializer.Serialize(genericErrorResponse);
        await httpContext.Response.WriteAsync(genericResult, cancellationToken);

        return true;
    }
}