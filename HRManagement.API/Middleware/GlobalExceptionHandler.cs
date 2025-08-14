using HRManagement.Core.Models;
using HRManagement.Infrastructure.Interfaces;
using System.Net;
using System.Text.Json;

namespace HRManagement.API.Middleware
{
    public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IDatabaseExceptionHandler databaseExceptionHandler)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;
        private readonly IDatabaseExceptionHandler _databaseExceptionHandler = databaseExceptionHandler;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (message, statusCode) = DetermineResponse(exception);
            
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = [exception.Message],
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = statusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private (string Message, int StatusCode) DetermineResponse(Exception exception)
        {
            // Handle database exceptions first
            if (IsDatabaseException(exception))
            {
                return _databaseExceptionHandler.HandleDatabaseException(exception);
            }

            // Handle other specific exceptions
            return exception switch
            {
                ArgumentNullException => (ExceptionMessages.Validation.RequiredField, (int)HttpStatusCode.BadRequest),
                ArgumentException => (exception.Message, (int)HttpStatusCode.BadRequest),
                InvalidOperationException => (exception.Message, (int)HttpStatusCode.BadRequest),
                UnauthorizedAccessException => (ExceptionMessages.General.Unauthorized, (int)HttpStatusCode.Unauthorized),
                KeyNotFoundException => (exception.Message, (int)HttpStatusCode.NotFound),
                FileNotFoundException => (ExceptionMessages.File.FileNotFound, (int)HttpStatusCode.NotFound),
                FormatException => (ExceptionMessages.Validation.InvalidFormat, (int)HttpStatusCode.BadRequest),
                _ => (ExceptionMessages.General.InternalServerError, (int)HttpStatusCode.InternalServerError),
            };
        }

        private bool IsDatabaseException(Exception exception)
        {
            // Check if it's a database-related exception
            if (exception is Microsoft.EntityFrameworkCore.DbUpdateException ||
                exception is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ||
                exception.GetType().Name == "SqlException" ||
                exception.GetType().Name.Contains("Postgres") ||
                exception.GetType().Name.Contains("Npgsql"))
            {
                return true;
            }

            // Check inner exceptions
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                if (IsDatabaseException(innerException))
                {
                    return true;
                }
                innerException = innerException.InnerException;
            }

            // Check message content for database-related keywords
            var message = exception.Message.ToLower();
            return message.Contains("database") || 
                   message.Contains("sql") || 
                   message.Contains("postgresql") ||
                   message.Contains("postgres") ||
                   message.Contains("npgsql") ||
                   message.Contains("constraint") || 
                   message.Contains("foreign key") || 
                   message.Contains("unique") || 
                   message.Contains("connection") ||
                   message.Contains("transaction") ||
                   // PostgreSQL specific error codes
                   message.Contains("23505") || // unique violation
                   message.Contains("23503") || // foreign key violation
                   message.Contains("23514") || // check violation
                   message.Contains("23502");  // not null violation
        }
    }
} 