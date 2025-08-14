using System.Data.SqlClient;
using System.Text.RegularExpressions;
using HRManagement.Core.Models;
using HRManagement.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Services
{
    /// <summary>
    /// Service to handle database exceptions and map them to user-friendly messages
    /// </summary>
    public class DatabaseExceptionHandler : IDatabaseExceptionHandler
    {
        /// <summary>
        /// Analyzes a database exception and returns a user-friendly error message
        /// </summary>
        /// <param name="ex">The database exception</param>
        /// <returns>User-friendly error message and HTTP status code</returns>
        public (string Message, int StatusCode) Handle(Exception ex)
        {
            // Handle Entity Framework Core exceptions
            if (ex is DbUpdateException dbUpdateEx)
            {
                return HandleDbUpdateException(dbUpdateEx);
            }

            // Handle SQL Server specific exceptions
            if (ex is SqlException sqlEx)
            {
                return HandleSqlException(sqlEx);
            }

            // Handle PostgreSQL specific exceptions
            if (IsPostgreSqlException(ex))
            {
                return HandlePostgreSqlException(ex);
            }

            // Handle Microsoft.Data.SqlClient exceptions (for newer versions)
            if (ex.GetType().Name == "SqlException" && ex.Message.Contains("SQL"))
            {
                return HandleGenericSqlException(ex);
            }

            // Handle connection exceptions
            if (ex.Message.Contains("connection") || ex.Message.Contains("Connection"))
            {
                return (ExceptionMessages.Database.ConnectionError, 503); // Service Unavailable
            }

            // Handle transaction exceptions
            if (ex.Message.Contains("transaction") || ex.Message.Contains("Transaction"))
            {
                return (ExceptionMessages.Database.TransactionError, 500);
            }

            // Default database error
            return (ExceptionMessages.Database.SaveChangesError, 500);
        }

        private (string Message, int StatusCode) HandleDbUpdateException(DbUpdateException ex)
        {
            var innerException = ex.InnerException;

            // Check for constraint violations
            if (innerException != null)
            {
                var message = innerException.Message.ToLower();

                // Unique constraint violations
                if (message.Contains("unique") || message.Contains("duplicate"))
                {
                    return (ExceptionMessages.Database.UniqueConstraintViolation, 409); // Conflict
                }

                // Foreign key constraint violations
                if (message.Contains("foreign key") || message.Contains("reference"))
                {
                    return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);
                }

                // Check constraint violations
                if (message.Contains("check") || message.Contains("constraint"))
                {
                    return (ExceptionMessages.Database.CheckConstraintViolation, 400);
                }

                // Not null constraint violations
                if (message.Contains("null") || message.Contains("not null"))
                {
                    return (ExceptionMessages.Database.NotNullConstraintViolation, 400);
                }
            }

            // Handle specific entity constraint violations
            var entityMessage = ex.Message.ToLower();
            if (entityMessage.Contains("employee"))
            {
                if (entityMessage.Contains("military") || entityMessage.Contains("number"))
                {
                    return (ExceptionMessages.Employee.MilitaryNumberExists, 409);
                }
                if (entityMessage.Contains("id") || entityMessage.Contains("identity"))
                {
                    return (ExceptionMessages.Employee.IdNumberExists, 409);
                }
                return (ExceptionMessages.Employee.AlreadyExists, 409);
            }

            if (entityMessage.Contains("role"))
            {
                if (entityMessage.Contains("code"))
                {
                    return (ExceptionMessages.Role.CodeExists, 409);
                }
                if (entityMessage.Contains("name"))
                {
                    return (ExceptionMessages.Role.NameExists, 409);
                }
                return (ExceptionMessages.Role.AlreadyExists, 409);
            }

            if (entityMessage.Contains("org") || entityMessage.Contains("unit"))
            {
                if (entityMessage.Contains("name"))
                {
                    return (ExceptionMessages.OrgUnit.NameExists, 409);
                }
                return (ExceptionMessages.OrgUnit.AlreadyExists, 409);
            }

            return (ExceptionMessages.Database.ConstraintViolation, 400);
        }

        private (string Message, int StatusCode) HandleSqlException(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627: // Unique constraint violation
                case 2601:
                    return (ExceptionMessages.Database.UniqueConstraintViolation, 409);

                case 547: // Foreign key constraint violation or Check constraint violation
                    if (ex.Message.ToLower().Contains("check"))
                    {
                        return (ExceptionMessages.Database.CheckConstraintViolation, 400);
                    }
                    return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);

                case 515: // Cannot insert NULL
                    return (ExceptionMessages.Database.NotNullConstraintViolation, 400);

                case 18456: // Login failed
                    return (ExceptionMessages.General.Unauthorized, 401);

                case 4060: // Cannot open database
                case 40613: // Database unavailable
                case 40501: // Service is currently busy
                    return (ExceptionMessages.Database.ConnectionError, 503);

                default:
                    return (ExceptionMessages.Database.SaveChangesError, 500);
            }
        }

        private bool IsPostgreSqlException(Exception ex)
        {
            // Check for Npgsql.PostgresException or similar PostgreSQL exception types
            var exceptionType = ex.GetType();
            return exceptionType.Name.Contains("Postgres") ||
                   exceptionType.Name.Contains("Npgsql") ||
                   exceptionType.FullName?.Contains("Npgsql") == true ||
                   ex.Message.Contains("PostgreSQL") ||
                   ex.Message.Contains("postgresql") ||
                   ex.Message.Contains("23505") || // Unique violation
                   ex.Message.Contains("23503") || // Foreign key violation
                   ex.Message.Contains("23514") || // Check violation
                   ex.Message.Contains("23502");  // Not null violation
        }

        private (string Message, int StatusCode) HandlePostgreSqlException(Exception ex)
        {
            var message = ex.Message.ToLower();

            // Extract PostgreSQL error code if present (e.g., "23505", "23503")
            var postgresErrorMatch = Regex.Match(ex.Message, @"(\d{5})");
            if (postgresErrorMatch.Success)
            {
                var errorCode = postgresErrorMatch.Groups[1].Value;
                return HandlePostgreSqlErrorCode(errorCode, ex);
            }

            // Fallback to message analysis
            if (message.Contains("unique") || message.Contains("duplicate") || message.Contains("already exists"))
            {
                return (ExceptionMessages.Database.UniqueConstraintViolation, 409);
            }

            if (message.Contains("foreign key") || message.Contains("reference") || message.Contains("violates foreign key"))
            {
                return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);
            }

            if (message.Contains("check") || message.Contains("constraint"))
            {
                return (ExceptionMessages.Database.CheckConstraintViolation, 400);
            }

            if (message.Contains("null") || message.Contains("not null") || message.Contains("violates not-null"))
            {
                return (ExceptionMessages.Database.NotNullConstraintViolation, 400);
            }

            if (message.Contains("connection") || message.Contains("could not connect"))
            {
                return (ExceptionMessages.Database.ConnectionError, 503);
            }

            if (message.Contains("authentication") || message.Contains("password") || message.Contains("login"))
            {
                return (ExceptionMessages.General.Unauthorized, 401);
            }

            return (ExceptionMessages.Database.ConstraintViolation, 400);
        }

        private (string Message, int StatusCode) HandlePostgreSqlErrorCode(string errorCode, Exception ex)
        {
            switch (errorCode)
            {
                case "23505": // unique_violation
                    return (ExceptionMessages.Database.UniqueConstraintViolation, 409);

                case "23503": // foreign_key_violation
                    return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);

                case "23514": // check_violation
                    return (ExceptionMessages.Database.CheckConstraintViolation, 400);

                case "23502": // not_null_violation
                    return (ExceptionMessages.Database.NotNullConstraintViolation, 400);

                case "23513": // exclusion_violation
                    return (ExceptionMessages.Database.CheckConstraintViolation, 400);

                case "42P01": // undefined_table
                case "42P02": // undefined_parameter
                case "42703": // undefined_column
                    return (ExceptionMessages.Database.CheckConstraintViolation, 400);

                case "28P01": // invalid_password
                case "28000": // invalid_authorization_specification
                    return (ExceptionMessages.General.Unauthorized, 401);

                case "08000": // connection_exception
                case "08001": // sqlclient_unable_to_establish_sqlconnection
                case "08003": // connection_does_not_exist
                case "08004": // sqlserver_rejected_establishment_of_sqlconnection
                case "08006": // connection_failure
                case "08007": // connection_failure_during_transaction
                    return (ExceptionMessages.Database.ConnectionError, 503);

                case "25P01": // invalid_transaction_state
                case "25P02": // active_sql_transaction
                case "25P03": // idle_in_transaction_session
                    return (ExceptionMessages.Database.TransactionError, 500);

                case "57014": // query_canceled
                case "57P01": // admin_shutdown
                case "57P02": // crash_shutdown
                case "57P03": // cannot_connect_now
                    return (ExceptionMessages.Database.ConnectionError, 503);

                default:
                    // For unknown error codes, analyze the message
                    var message = ex.Message.ToLower();
                    if (message.Contains("unique") || message.Contains("duplicate"))
                    {
                        return (ExceptionMessages.Database.UniqueConstraintViolation, 409);
                    }
                    if (message.Contains("foreign key") || message.Contains("reference"))
                    {
                        return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);
                    }
                    if (message.Contains("null") || message.Contains("not null"))
                    {
                        return (ExceptionMessages.Database.NotNullConstraintViolation, 400);
                    }
                    return (ExceptionMessages.Database.ConstraintViolation, 400);
            }
        }

        private (string Message, int StatusCode) HandleGenericSqlException(Exception ex)
        {
            var message = ex.Message.ToLower();

            // Extract SQL error number if present
            var match = Regex.Match(ex.Message, @"Error (\d+):");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int errorNumber))
            {
                // Create a mock SqlException for error number handling
                try
                {
                    // Use reflection to create SqlException if possible
                    var sqlExceptionType = typeof(SqlException);
                    var constructor = sqlExceptionType.GetConstructor(new[] { typeof(string), typeof(Exception), typeof(int), typeof(object) });
                    if (constructor != null)
                    {
                        var sqlEx = (SqlException)constructor.Invoke(new object[] { ex.Message, ex, errorNumber, null! });
                        return HandleSqlException(sqlEx);
                    }
                }
                catch
                {
                    // Fallback to message analysis if constructor fails
                }
            }

            // Fallback to message analysis
            if (message.Contains("unique") || message.Contains("duplicate"))
            {
                return (ExceptionMessages.Database.UniqueConstraintViolation, 409);
            }

            if (message.Contains("foreign key") || message.Contains("reference"))
            {
                return (ExceptionMessages.Database.ForeignKeyConstraintViolation, 409);
            }

            if (message.Contains("null") || message.Contains("not null"))
            {
                return (ExceptionMessages.Database.NotNullConstraintViolation, 400);
            }

            return (ExceptionMessages.Database.ConstraintViolation, 400);
        }
    }
}
