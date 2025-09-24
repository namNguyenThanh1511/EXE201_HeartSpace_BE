using HeartSpace.Api.Services;
using HeartSpace.Application.Exceptions;
using HeartSpace.Domain.Exception;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace HeartSpace.Api.Middleware
{
    public class GlobalExceptionMiddleware : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var response = exception switch
            {
                // SPECIFIC Domain Exceptions FIRST (most derived)
                UserAlreadyExistsException existsEx => ResponseBuilder.BadRequest(existsEx.Message),
                AccountLockedException lockedEx => ResponseBuilder.Error(lockedEx.Message, 423),
                InsufficientPermissionException permissionEx => ResponseBuilder.Forbidden(permissionEx.Message),

                // THEN Business Rule Violations (derived from BaseDomainException)
                BusinessRuleViolationException businessEx => ResponseBuilder.BadRequest(businessEx.Message),

                // OTHER Domain Exceptions (derived from BaseDomainException)
                EntityNotFoundException notFoundEx => ResponseBuilder.NotFound(notFoundEx.Message),
                InvalidCredentialsException credentialsEx => ResponseBuilder.Unauthorized(credentialsEx.Message),
                UserInactiveException inactiveEx => ResponseBuilder.Unauthorized(inactiveEx.Message),

                // Application Exceptions (specific first)
                ExternalServiceException serviceEx => ResponseBuilder.Error(serviceEx.Message, 502),
                DatabaseException dbEx => ResponseBuilder.InternalServerError("A database error occurred"),
                Application.Exceptions.UnauthorizedAccessException unauthorizedEx => ResponseBuilder.Unauthorized(unauthorizedEx.Message),
                ForbiddenAccessException forbiddenEx => ResponseBuilder.Forbidden(forbiddenEx.Message),

                // GENERIC Base Exceptions LAST (most base)
                BaseDomainException domainEx => ResponseBuilder.Error(domainEx.Message, domainEx.HttpStatusCode),
                BaseApplicationException appEx => ResponseBuilder.Error(appEx.Message, appEx.HttpStatusCode),

                // Fallback for any unexpected exception
                _ => ResponseBuilder.InternalServerError(exception.Message)
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = response.Code;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            return true; // ✅ Exception handled
        }
    }
}
