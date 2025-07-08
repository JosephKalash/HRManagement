using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ErrorResponse Create(string title, string detail, int status = 400, string type = "https://tools.ietf.org/html/rfc7231#section-6.5.1")
        {
            return new ErrorResponse
            {
                Type = type,
                Title = title,
                Status = status,
                Detail = detail
            };
        }

        public static ErrorResponse ValidationError(List<ValidationError> errors, string detail = "One or more validation errors occurred.")
        {
            return new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation Error",
                Status = 400,
                Detail = detail,
                Errors = errors
            };
        }
    }

    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
} 