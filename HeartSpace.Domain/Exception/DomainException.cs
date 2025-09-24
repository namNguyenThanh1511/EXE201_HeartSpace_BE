namespace HeartSpace.Domain.Exception
{
    public abstract class BaseDomainException : System.Exception
    {
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        protected BaseDomainException(string message, string errorCode, int httpStatusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }
    }

    // Business Rule Violations (không phải validation)
    public class BusinessRuleViolationException : BaseDomainException
    {
        public BusinessRuleViolationException(string message, string errorCode = "BUSINESS_RULE_VIOLATION")
            : base(message, errorCode, 400) { }
    }

    // Entity Not Found
    public class EntityNotFoundException : BaseDomainException
    {
        public EntityNotFoundException(string entityName, object id)
            : base($"{entityName} with ID {id} was not found", "ENTITY_NOT_FOUND", 404) { }

        public EntityNotFoundException(string message)
            : base(message, "ENTITY_NOT_FOUND", 404) { }
    }

    // Authentication/Authorization Domain Exceptions
    public class InvalidCredentialsException : BaseDomainException
    {
        public InvalidCredentialsException(string message = "Invalid username or password")
            : base(message, "INVALID_CREDENTIALS", 401) { }
    }

    public class UserInactiveException : BaseDomainException
    {
        public UserInactiveException(string message = "User account is inactive")
            : base(message, "USER_INACTIVE", 401) { }
    }

    // Specific Business Rule Exceptions
    public class UserAlreadyExistsException : BusinessRuleViolationException
    {
        public UserAlreadyExistsException(string field, string value)
            : base($"User with {field} '{value}' already exists", "USER_ALREADY_EXISTS") { }
    }

    public class InsufficientPermissionException : BaseDomainException
    {
        public InsufficientPermissionException(string message = "Insufficient permission to perform this action")
            : base(message, "INSUFFICIENT_PERMISSION", 403) { }
    }

    public class AccountLockedException : BaseDomainException
    {
        public AccountLockedException(string message = "Account is locked")
            : base(message, "ACCOUNT_LOCKED", 423) { }
    }
}
