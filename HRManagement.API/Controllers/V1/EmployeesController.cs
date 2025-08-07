using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        /// <response code="200">Returns the list of employees</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeDto>>>> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<EmployeeDto>>.SuccessResult(pagedResult, "Employees retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeDto>>.ErrorResult("An error occurred while retrieving employees", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        /// <response code="200">Returns the employee</response>
        /// <response code="404">If employee is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployee(Guid id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(ApiResponse<EmployeeDto>.ErrorResult($"Employee with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeDto>.ErrorResult("An error occurred while retrieving the employee", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get active employees only
        /// </summary>
        /// <returns>List of active employees</returns>
        /// <response code="200">Returns the list of active employees</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetActiveEmployees()
        {
            try
            {
                var employees = await _employeeService.GetActiveEmployeesAsync();
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, "Active employees retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeDto>>.ErrorResult("An error occurred while retrieving active employees", new List<string> { ex.Message }));
            }
        }



        /// <summary>
        /// Search employees
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching employees</returns>
        /// <response code="200">Returns the list of matching employees</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> SearchEmployees([FromQuery] string searchTerm)
        {
            try
            {
                var employees = await _employeeService.SearchEmployeesAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, $"Search results for '{searchTerm}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeDto>>.ErrorResult("An error occurred while searching employees", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="createDto">Employee creation data</param>
        /// <returns>Created employee</returns>
        /// <response code="201">Returns the created employee</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee([FromBody] CreateEmployeeDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeDto>.ErrorResult("Validation failed", errors));
                }

                var employee = await _employeeService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id },
                    ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeDto>.ErrorResult("An error occurred while creating the employee", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="updateDto">Employee update data</param>
        /// <returns>Updated employee</returns>
        /// <response code="200">Returns the updated employee</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If employee is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(Guid id, UpdateEmployeeDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeDto>.ErrorResult("Validation failed", errors));
                }

                var employee = await _employeeService.UpdateAsync(id, updateDto);
                return Ok(ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeDto>.ErrorResult("An error occurred while updating the employee", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Employee deleted successfully</response>
        /// <response code="404">If employee is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteEmployee(Guid id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the employee", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Upload profile image for an employee
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <param name="file">Image file</param>
        /// <returns>Upload result</returns>
        /// <response code="200">Image uploaded successfully</response>
        /// <response code="400">If the file is invalid or employee not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<object>>> UploadProfileImage(Guid employeeId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse.ErrorResult("No file uploaded"));

                using var stream = file.OpenReadStream();
                var filePath = await _employeeService.UploadProfileImageAsync(employeeId, stream, file.FileName);

                return Ok(ApiResponse<object>.SuccessResult(new { filePath }, "Image uploaded successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while uploading the image", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get profile image for an employee
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Image file</returns>
        /// <response code="200">Returns the image file</response>
        /// <response code="404">If image is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<IActionResult> GetProfileImage(Guid employeeId)
        {
            try
            {
                var employee = await _employeeService.GetProfileByEmployeeIdAsync(employeeId);
                if (employee == null || string.IsNullOrEmpty(employee.ImagePath))
                    return NotFound(ApiResponse.ErrorResult("Profile image not found"));

                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", employee.ImagePath.Replace('/', Path.DirectorySeparatorChar));

                if (!System.IO.File.Exists(filePath))
                    return NotFound(ApiResponse.ErrorResult("Image file not found on disk"));

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = GetContentType(employee.ImagePath);

                return File(fileBytes, contentType);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while retrieving the image", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete profile image for an employee
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">Image deleted successfully</response>
        /// <response code="400">If employee not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteProfileImage(Guid employeeId)
        {
            try
            {
                await _employeeService.DeleteProfileImageAsync(employeeId);
                return Ok(ApiResponse.SuccessResult("Image deleted successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the image", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get comprehensive employee details by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Comprehensive employee details</returns>
        /// <response code="200">Returns the comprehensive employee details</response>
        /// <response code="404">If employee is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(ApiResponse<ComprehensiveEmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<ComprehensiveEmployeeDto>>> GetComprehensiveEmployeeDetails(Guid id)
        {
            try
            {
                var employee = await _employeeService.GetComprehensiveEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(ApiResponse<ComprehensiveEmployeeDto>.ErrorResult($"Employee with ID {id} not found"));
                }

                return Ok(ApiResponse<ComprehensiveEmployeeDto>.SuccessResult(employee, "Comprehensive employee details retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ComprehensiveEmployeeDto>.ErrorResult("An error occurred while retrieving comprehensive employee details", new List<string> { ex.Message }));
            }
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}