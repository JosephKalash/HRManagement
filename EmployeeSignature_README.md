# Employee Signature Feature

## Overview
The Employee Signature feature allows storing and managing employee signature images with associated metadata. Each employee can have one signature, and signatures are stored as image files in the system.

## Architecture
This feature follows the clean architecture pattern used in the HR Management system:

### Core Layer (`HRManagement.Core`)
- **Entity**: `EmployeeSignature` - Core business entity
- **Interface**: `IEmployeeSignatureRepository` - Repository contract

### Application Layer (`HRManagement.Application`)
- **DTOs**: 
  - `EmployeeSignatureDto` - Data transfer object for signatures
  - `CreateEmployeeSignatureDto` - DTO for creating signatures
  - `UpdateEmployeeSignatureDto` - DTO for updating signatures
- **Service**: `EmployeeSignatureService` - Business logic implementation
- **Interface**: `IEmployeeSignatureService` - Service contract

### Infrastructure Layer (`HRManagement.Infrastructure`)
- **Repository**: `EmployeeSignatureRepository` - Data access implementation
- **DbContext**: Updated to include `EmployeeSignatures` DbSet

### API Layer (`HRManagement.API`)
- **Controller**: `EmployeeSignaturesController` - REST API endpoints
- **Models**: API request/response models for signature operations

## Features

### 1. Create Employee Signature
- **Endpoint**: `POST /api/v1/EmployeeSignatures`
- **Content-Type**: `multipart/form-data`
- **Parameters**:
  - `EmployeeId` (required): GUID of the employee
  - `SignatureName` (required): Name/description of the signature
  - `Image` (required): Image file (jpg, jpeg, png, gif)

### 2. Get Employee Signature
- **Endpoint**: `GET /api/v1/EmployeeSignatures/{id}`
- **Returns**: Signature details with full image URL

### 3. Get Employee Signature by Employee ID
- **Endpoint**: `GET /api/v1/EmployeeSignatures/employee/{employeeId}`
- **Returns**: Signature details for a specific employee

### 4. Update Employee Signature
- **Endpoint**: `PATCH /api/v1/EmployeeSignatures/{id}`
- **Parameters**: `SignatureName` (optional)

### 5. Update Signature Image
- **Endpoint**: `PATCH /api/v1/EmployeeSignatures/{id}/image`
- **Content-Type**: `multipart/form-data`
- **Parameters**: `Image` (required) - New image file

### 6. Delete Employee Signature
- **Endpoint**: `DELETE /api/v1/EmployeeSignatures/{id}`
- **Note**: Also deletes the associated image file

### 7. List All Signatures (Paginated)
- **Endpoint**: `GET /api/v1/EmployeeSignatures?pageNumber=1&pageSize=10`

## Database Schema

### EmployeeSignature Table
```sql
CREATE TABLE EmployeeSignatures (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    SignatureName NVARCHAR(100) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    OriginalFileName NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE
);
```

### Relationships
- **Employee** ↔ **EmployeeSignature**: One-to-One relationship
- Each employee can have at most one signature
- When an employee is deleted, their signature is automatically deleted (CASCADE)

## File Storage
- **Location**: `wwwroot/uploads/signatures/`
- **File Naming**: `yyyyMMdd_HHmmss_{GUID}.{extension}`
- **Supported Formats**: JPG, JPEG, PNG, GIF
- **Maximum Size**: 10MB per file
- **URL Structure**: `{baseUrl}/uploads/signatures/{filename}`

## Validation Rules
- **EmployeeId**: Must exist in the system
- **SignatureName**: Required, max 100 characters
- **Image**: Required for creation, must be valid image format
- **Uniqueness**: Only one signature per employee

## Error Handling
- **400 Bad Request**: Invalid input data or missing required fields
- **404 Not Found**: Employee or signature not found
- **500 Internal Server Error**: Server-side errors

## Usage Examples

### Creating a Signature
```bash
curl -X POST "https://localhost:5001/api/v1/EmployeeSignatures" \
  -H "Content-Type: multipart/form-data" \
  -F "EmployeeId=123e4567-e89b-12d3-a456-426614174000" \
  -F "SignatureName=Official Signature" \
  -F "Image=@signature.png"
```

### Getting a Signature
```bash
curl -X GET "https://localhost:5001/api/v1/EmployeeSignatures/123e4567-e89b-12d3-a456-426614174000"
```

### Updating Signature Name
```bash
curl -X PATCH "https://localhost:5001/api/v1/EmployeeSignatures/123e4567-e89b-12d3-a456-426614174000" \
  -H "Content-Type: application/json" \
  -d '{"SignatureName": "Updated Signature Name"}'
```

## Integration with Existing System
- **ComprehensiveEmployeeDto**: Now includes signature information
- **AutoMapper**: Configured for all signature-related mappings
- **Dependency Injection**: All services and repositories properly registered
- **Global Exception Handling**: Consistent error responses

## Security Considerations
- File upload validation (format, size)
- Secure file storage outside web root
- Input validation and sanitization
- Proper error messages without information disclosure

## Future Enhancements
- Signature versioning
- Digital signature validation
- Signature approval workflow
- Bulk signature operations
- Signature templates
- Audit logging for signature changes
