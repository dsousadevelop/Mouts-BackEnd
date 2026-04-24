namespace Ambev.DeveloperEvaluation.Application.Common.Errors
{
    public class BaseError
    {
        public string Type { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }

    public class ResourceNotFoundError : BaseError
    {
        public ResourceNotFoundError(string detail)
        {
            Type = "ResourceNotFound";
            Error = "Resource not found";
            Detail = detail;
        }
    }

    public class NotFoundError : BaseError
    {
        public NotFoundError(string detail)
        {
            Type = "NotFound";
            Error = "Resource not found";
            Detail = detail;
        }
    }

    public class ValidationError : BaseError
    {
        public ValidationError(string detail)
        {
            Type = "ValidationError";
            Error = "Invalid input data";
            Detail = detail;
        }
    }

    public class AuthenticationError : BaseError
    {
        public AuthenticationError(string detail)
        {
            Type = "AuthenticationError";
            Error = "Invalid authentication token";
            Detail = detail;
        }
    }
}
