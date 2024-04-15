using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Utils.Errors;
using BoxCommerce.Utils.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Net;
using System.Text.Json;

namespace BoxCommerce.Orders.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _hostingEnv;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostingEnv)
    {
        _next = next;
        _logger = logger;
        _hostingEnv = hostingEnv;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        ErrorDetails errorDetails = new()
        {
            Succeeded = false
        };

        var _currentUserService = context.RequestServices.GetRequiredService<ICurrentUserService>();

        var propertyDictionary = new Dictionary<string, object>
        {
            { "UserId", _currentUserService.UserId ?? string.Empty },
        };
        // Add More Cases

        using (_logger.BeginScope(propertyDictionary))
        {
            if (exception is AppException)
            {
                var appException = exception as AppException;
                errorDetails.ErrorMessage = appException.Error.ErrorKey;
                errorDetails.ErrorExceptionMessage = appException.Error.Message;
                response.StatusCode = (int)appException.Error.StatusCode;
                _logger.LogWarning(exception, "Application Error in Custom Middleware");
            }
            else if (exception is ValidationException)
            {
                var validationException = exception as ValidationException;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails.ErrorMessage = "validation_failed";
                errorDetails.ErrorExceptionMessage = validationException.Message;
                errorDetails.Errors = validationException.Errors.Select(x => new Error { ErrorMessage = x.PropertyName, ErrorExceptionMessage = x.ErrorMessage }).ToList();
                _logger.LogWarning(validationException, "A validation error occurred");
            }
            else if (exception is BadHttpRequestException)
            {
                errorDetails.ErrorMessage = "bad_request";
                errorDetails.ErrorExceptionMessage = exception.Message;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogError(exception, "A bad request was received");
            }
            else if (exception is UnauthorizedAccessException)
            {
                errorDetails.ErrorMessage = "unauthorized_access";
                errorDetails.ErrorExceptionMessage = exception.Message;
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                _logger.LogError(exception, "Unauthorized exception occurred");
            }
            else if (exception is DbUpdateException)
            {
                if (exception.InnerException is DbException dbException)
                {
                    errorDetails.ErrorMessage = "unique_constraint_violation";
                    errorDetails.ErrorExceptionMessage = "A conflict occured.";
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    _logger.LogError(exception, "Db exception occurred");
                }
                else
                {
                    errorDetails.ErrorMessage = "dbupdate_failed";
                    errorDetails.ErrorExceptionMessage = "Db update failed.";
                    response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    _logger.LogError(exception, "DbUpdate exception occurred");
                }
            }
            else
            {
                errorDetails.ErrorMessage = "something_went_wrong";
                errorDetails.ErrorExceptionMessage = "Something went wrong";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "An unhandled exception occurred");

            }

            var env = _hostingEnv.EnvironmentName;
            var env1 = _hostingEnv.IsDevelopment();

            if (_hostingEnv.IsDevelopment() && (!string.IsNullOrEmpty(errorDetails.ErrorMessage) || !string.IsNullOrEmpty(errorDetails.ErrorExceptionMessage)))
            {
                errorDetails.ErrorDetails = exception.Message;
                errorDetails.InnerException = exception?.InnerException?.Message;
                errorDetails.StackTrace = exception?.StackTrace;
            }

            string responseText = JsonSerializer.Serialize(errorDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            await response.WriteAsync(responseText);
        }
    }

}
