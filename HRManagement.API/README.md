# HR Management API

A comprehensive ASP.NET Core 8.0 HR management system built with clean architecture, Entity Framework Core, and SQL Server.

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **API Layer**: Controllers, middleware, and configuration
- **Application Layer**: Business logic, DTOs, and services
- **Infrastructure Layer**: Data access, repositories, and external services
- **Core Layer**: Entities, interfaces, and domain models

## 🚀 Features

- **Employee Management**: CRUD operations for employee data
- **Leave Request Management**: Handle employee leave requests
- **Performance Reviews**: Track employee performance evaluations
- **API Versioning**: Support for multiple API versions
- **Swagger Documentation**: Interactive API documentation
- **SQL Server Integration**: Entity Framework Core with SQL Server
- **Clean Architecture**: Separation of concerns and maintainable code
- **Response Templates**: Consistent API response format
- **Error Handling**: Comprehensive error responses

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## 🛠️ Setup Instructions

### 1. Clone and Navigate
```bash
cd HRManagement.API
```

### 2. Update Connection String
Edit `appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HRManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Restore Dependencies
```bash
dotnet restore
```

### 4. Run Database Migrations
```bash
dotnet ef database update
```

### 5. Run the Application
```bash
dotnet run
```

The API will be available at:
- **API**: https://localhost:7001
- **Swagger UI**: https://localhost:7001 (root)

## 📚 API Documentation

### Base URL
```
https://localhost:7001/api/v1
```

### Endpoints

#### Employees
- `GET /employees` - Get all employees
- `GET /employees/{id}` - Get employee by ID
- `GET /employees/active` - Get active employees only
- `GET /employees/department/{department}` - Get employees by department
- `GET /employees/search?searchTerm={term}` - Search employees
- `POST /employees` - Create new employee
- `PUT /employees/{id}` - Update employee
- `DELETE /employees/{id}` - Delete employee

#### Leave Requests
- `GET /leaverequests` - Get all leave requests
- `GET /leaverequests/{id}` - Get leave request by ID
- `GET /leaverequests/employee/{employeeId}` - Get leave requests by employee
- `GET /leaverequests/status/{status}` - Get leave requests by status
- `POST /leaverequests` - Create new leave request
- `PUT /leaverequests/{id}` - Update leave request
- `DELETE /leaverequests/{id}` - Delete leave request

#### Performance Reviews
- `GET /performancereviews` - Get all performance reviews
- `GET /performancereviews/{id}` - Get performance review by ID
- `GET /performancereviews/employee/{employeeId}` - Get reviews by employee
- `POST /performancereviews` - Create new performance review
- `PUT /performancereviews/{id}` - Update performance review
- `DELETE /performancereviews/{id}` - Delete performance review

## 📊 Database Schema

### Employees Table
- Id (Primary Key)
- FirstName
- LastName
- Email (Unique)
- PhoneNumber
- DateOfBirth
- HireDate
- Position
- Department
- Salary
- IsActive
- CreatedAt
- UpdatedAt

### LeaveRequests Table
- Id (Primary Key)
- EmployeeId (Foreign Key)
- StartDate
- EndDate
- LeaveType (Enum)
- Reason
- Status (Enum)
- ManagerComments
- CreatedAt
- UpdatedAt

### PerformanceReviews Table
- Id (Primary Key)
- EmployeeId (Foreign Key)
- ReviewDate
- ReviewerName
- OverallRating (1-5)
- Strengths
- AreasForImprovement
- Goals
- Comments
- CreatedAt
- UpdatedAt

## 🔧 Configuration

### API Versioning
The API supports versioning through URL path:
- `/api/v1/employees` - Version 1.0
- `/api/v2/employees` - Version 2.0 (future)

### Response Format
All API responses follow a consistent format:

```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { ... },
  "errors": [],
  "timestamp": "2024-01-01T00:00:00Z"
}
```

### Error Handling
Errors are returned with appropriate HTTP status codes and detailed messages:

```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": ["Field is required"],
  "timestamp": "2024-01-01T00:00:00Z"
}
```

## 🧪 Testing

### Using Swagger UI
1. Navigate to the root URL
2. Use the interactive Swagger documentation
3. Test endpoints directly from the browser

### Using Postman
1. Import the provided Postman collection
2. Set the base URL to `https://localhost:7001/api/v1`
3. Test all endpoints

## 📁 Project Structure

```
HRManagement.API/
├── Controllers/
│   └── V1/
│       └── EmployeesController.cs
├── HRManagement.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── HRManagement.Core/
│   ├── Entities/
│   ├── Interfaces/
│   └── Models/
├── HRManagement.Infrastructure/
│   ├── Data/
│   └── Repositories/
├── Program.cs
├── appsettings.json
└── README.md
```

## 🔒 Security Considerations

- Use HTTPS in production
- Implement authentication and authorization
- Validate all input data
- Use parameterized queries (EF Core handles this)
- Implement rate limiting for production

## 🚀 Deployment

### Local Development
```bash
dotnet run
```

### Production
```bash
dotnet publish -c Release
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 📞 Support

For support and questions:
- Email: hr@company.com
- Documentation: Available in Swagger UI
- Issues: Create an issue in the repository 