namespace HeartSpace.Application.Exceptions
{
    // Application Service Layer Exceptions
    public abstract class BaseApplicationException : Exception
    {
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        protected BaseApplicationException(string message, string errorCode, int httpStatusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }
    }

    // Authorization/Authentication (Application Level)
    public class UnauthorizedAccessException : BaseApplicationException
    {
        public UnauthorizedAccessException(string message = "Unauthorized access")
            : base(message, "UNAUTHORIZED_ACCESS", 401) { }
    }

    public class ForbiddenAccessException : BaseApplicationException
    {
        public ForbiddenAccessException(string message = "Access forbidden")
            : base(message, "FORBIDDEN_ACCESS", 403) { }
    }

    // External Service Exceptions
    public class ExternalServiceException : BaseApplicationException
    {
        public ExternalServiceException(string serviceName, string message)
            : base($"External service '{serviceName}' error: {message}", "EXTERNAL_SERVICE_ERROR", 502) { }
    }

    // Infrastructure Exceptions
    public class DatabaseException : BaseApplicationException
    {
        public DatabaseException(string message, Exception innerException)
            : base($"Database error: {message}", "DATABASE_ERROR", 500) { }
    }
}
