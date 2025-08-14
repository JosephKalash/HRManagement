namespace HRManagement.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for handling database exceptions and mapping them to user-friendly messages
    /// </summary>
    public interface IDatabaseExceptionHandler
    {
        /// <summary>
        /// Analyzes a database exception and returns a user-friendly error message
        /// </summary>
        /// <param name="ex">The database exception</param>
        /// <returns>User-friendly error message and HTTP status code</returns>
        (string Message, int StatusCode) HandleDatabaseException(Exception ex);
    }
}
