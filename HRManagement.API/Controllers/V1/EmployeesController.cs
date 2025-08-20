using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeesController(IEmployeeService employeeService, ICurrentUserService currentUserService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ICurrentUserService _currentUserService = currentUserService;







        [HttpGet]
        [OutputCache(PolicyName = "EmployeesPaged")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeDto>>>> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<EmployeeDto>>.SuccessResult(pagedResult, "Employees retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }









        [HttpGet("me")]
        [OutputCache(PolicyName = "CurrentEmployee")]
        [ProducesResponseType(typeof(ApiResponse<CurrentEmployeeSummaryDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<ApiResponse<CurrentEmployeeSummaryDto>>> GetCurrentEmployee()
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (!userId.HasValue)
                {
                    return Unauthorized(ApiResponse.ErrorResult("User is not authenticated"));
                }

                var summary = await _employeeService.GetCurrentEmployeeSummaryAsync(userId.Value);
                if (summary == null)
                {
                    return NotFound(ApiResponse<CurrentEmployeeSummaryDto>.ErrorResult("Employee not found for current user"));
                }

                return Ok(ApiResponse<CurrentEmployeeSummaryDto>.SuccessResult(summary, "Current employee retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpGet("{id}")]
        [OutputCache(PolicyName = "EmployeeById")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
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
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpGet("short/{id}")]
        [OutputCache(PolicyName = "ShortEmployeeById")]
        [ProducesResponseType(typeof(ApiResponse<ShortEmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<ShortEmployeeDto>>> GetShortEmployee(Guid id)
        {
            try
            {
                var employee = await _employeeService.GetByIdShort(id);
                return Ok(ApiResponse<ShortEmployeeDto>.SuccessResult(employee, "Employee retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }
        
        [HttpPost("short/list")]
        [ProducesResponseType(typeof(ApiResponse<List<ShortEmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<List<ShortEmployeeDto>>>> GetShortEmployee(List<Guid> ids)
        {
            try
            {
                var employee = await _employeeService.GetByIdsShortAsync(ids);
                return Ok(ApiResponse<List<ShortEmployeeDto>>.SuccessResult(employee, "Employee retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpGet("short/by-military/{militaryNumber}")]
        [OutputCache(PolicyName = "ShortEmployeeByMilitary")]
        [ProducesResponseType(typeof(ApiResponse<ShortEmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<ShortEmployeeDto>>> GetShortEmployee(int militaryNumber)
        {
            try
            {
                var employee = await _employeeService.GetByMilitaryShort(militaryNumber);
                return Ok(ApiResponse<ShortEmployeeDto>.SuccessResult(employee, "Employee retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpPost("short/list/by-military")]
        [ProducesResponseType(typeof(ApiResponse<List<ShortEmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<List<ShortEmployeeDto>>>> GetShortEmployee(List<int> militaryNumbers)
        {
            try
            {
                var employee = await _employeeService.GetByMilitariesShortList(militaryNumbers);
                return Ok(ApiResponse<List<ShortEmployeeDto>>.SuccessResult(employee, "Employee retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpGet("role-ids/{employeeId}")]
        [OutputCache(PolicyName = "EmployeeRoleIds")]
        [ProducesResponseType(typeof(ApiResponse<List<Guid>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<List<Guid>>>> GetEmployeeRoleIds(Guid employeeId)
        {
            try
            {
                var ids = await _employeeService.GetEmployeeRoleIds(employeeId);
                return Ok(ApiResponse<List<Guid>>.SuccessResult(ids, "Employee roles ids retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpGet("active")]
        [OutputCache(PolicyName = "ActiveEmployees")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 400)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetActiveEmployees()
        {
            try
            {
                var employees = await _employeeService.GetActiveEmployeesAsync();
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, "Active employees retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }








        [HttpGet("search")]
        [OutputCache(PolicyName = "SearchEmployees")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> SearchEmployees([FromQuery] string searchTerm)
        {
            try
            {
                var employees = await _employeeService.SearchEmployeesAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, $"Search results for '{searchTerm}' retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

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
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }





        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

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
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<List<Guid>>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

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

        [HttpPost("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

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

        [HttpGet("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

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









        [HttpDelete("{employeeId}/profile-image")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

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









        [HttpGet("{id}/details")]
        [OutputCache(PolicyName = "EmployeeDetails")]
        [ProducesResponseType(typeof(ApiResponse<ComprehensiveEmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

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








        [HttpGet("{employeeId}/job-summary")]
        [OutputCache(PolicyName = "EmployeeJobSummary")]
        [ProducesResponseType(typeof(ApiResponse<List<EmployeeJobSummaryDto>>), 200)]

        public async Task<ActionResult<ApiResponse<List<EmployeeJobSummaryDto>>>> GetEmployeeJobSummary(Guid employeeId)
        {
            try
            {
                var employeeExists = await _employeeService.ExistsAsync(employeeId);
                if (!employeeExists)
                {
                    return NotFound(ApiResponse<List<EmployeeJobSummaryDto>>.ErrorResult($"Employee with ID {employeeId} not found"));
                }

                var jobSummaries = await _employeeService.GetEmployeeJobSummaryAsync(employeeId);
                return Ok(ApiResponse<List<EmployeeJobSummaryDto>>.SuccessResult(jobSummaries, "Employee job summary retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<EmployeeJobSummaryDto>>.ErrorResult("An error occurred while retrieving employee job summary", new List<string> { ex.Message }));
            }
        }








        [HttpGet("by-role/{roleId}")]
        [OutputCache(PolicyName = "EmployeesByRole")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetEmployeesByRoleId(Guid roleId)
        {
            try
            {
                var employees = await _employeeService.GetEmployeeByRoleIdAsync(roleId);
                return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, "Employees with the specified role retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeDto>>.ErrorResult("An error occurred while retrieving employees by role", new List<string> { ex.Message }));
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