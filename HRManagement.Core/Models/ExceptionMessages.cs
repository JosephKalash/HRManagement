namespace HRManagement.Core.Models
{
    /// <summary>
    /// Centralized exception messages for the application
    /// </summary>
    public static class ExceptionMessages
    {
        // General application messages
        public static class General
        {
            public const string UnexpectedError = "An unexpected error occurred";
            public const string InternalServerError = "An internal server error occurred";
            public const string ContactSupport = "Please try again later or contact support if the problem persists";
            public const string ValidationError = "Validation error occurred";
            public const string NotFound = "The requested resource was not found";
            public const string Unauthorized = "Unauthorized access";
            public const string Forbidden = "Access forbidden";
            public const string BadRequest = "Invalid request";
        }

        // Database-related messages
        public static class Database
        {
            public const string ConstraintViolation = "Database constraint violation occurred";
            public const string UniqueConstraintViolation = "A record with this information already exists";
            public const string ForeignKeyConstraintViolation = "Cannot delete this record as it is referenced by other records";
            public const string CheckConstraintViolation = "The data does not meet the required constraints";
            public const string NotNullConstraintViolation = "Required field cannot be null";
            public const string ConnectionError = "Database connection error occurred";
            public const string TransactionError = "Database transaction error occurred";
            public const string SaveChangesError = "Error occurred while saving changes to the database";
        }

        // Entity-specific messages
        public static class Employee
        {
            public const string NotFound = "Employee not found";
            public const string AlreadyExists = "An employee with this information already exists";
            public const string MilitaryNumberExists = "An employee with this military number already exists";
            public const string IdNumberExists = "An employee with this ID number already exists";
            public const string CannotDelete = "Cannot delete employee as they have associated records";
        }

        public static class Role
        {
            public const string NotFound = "Role not found";
            public const string CodeExists = "A role with this code already exists";
            public const string NameExists = "A role with this name already exists";
            public const string AlreadyExists = "A role with this information already exists";
            public const string CannotDelete = "Cannot delete role as it is assigned to employees";
        }

        public static class OrgUnit
        {
            public const string NotFound = "Organizational unit not found";
            public const string NameExists = "An organizational unit with this name already exists";
            public const string AlreadyExists = "An organizational unit with this information already exists";
            public const string CannotDelete = "Cannot delete organizational unit as it has employees or child units";
            public const string CircularReference = "Circular reference detected in organizational hierarchy";
        }

        // Validation messages
        public static class Validation
        {
            public const string RequiredField = "This field is required";
            public const string InvalidFormat = "Invalid format";
            public const string InvalidLength = "Invalid length";
            public const string InvalidValue = "Invalid value";
            public const string FutureDateNotAllowed = "Future date is not allowed";
            public const string PastDateNotAllowed = "Past date is not allowed";
        }

        // File operation messages
        public static class File
        {
            public const string UploadFailed = "File upload failed";
            public const string DeleteFailed = "File deletion failed";
            public const string InvalidFileType = "Invalid file type";
            public const string FileTooLarge = "File size exceeds the maximum allowed limit";
            public const string FileNotFound = "File not found";
        }
    }
}
