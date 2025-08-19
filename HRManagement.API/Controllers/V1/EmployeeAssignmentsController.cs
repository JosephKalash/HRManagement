using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeeAssignmentsController(IEmployeeAssignmentService employeeAssignmentService, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeAssignmentService _employeeAssignmentService = employeeAssignmentService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all employee assignments
        /// </summary>
        /// <returns>List of all employee assignments</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeAssignmentDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeAssignmentDto>>>> GetEmployeeAssignments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeAssignmentService.GetPagedAsync(pageNumber, pageSize);
                var dtos = _mapper.Map<List<EmployeeAssignmentDto>>(pagedResult.Items);
                return Ok(ApiResponse<PagedResult<EmployeeAssignmentDto>>.SuccessResult(new PagedResult<EmployeeAssignmentDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee assignments retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeAssignmentDto>>.ErrorResult("An error occurred while retrieving employee assignments", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee assignment by ID
        /// </summary>
        /// <param name="id">Employee Assignment ID</param>
        /// <returns>Employee assignment details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> GetEmployeeAssignment(Guid id)
        {
            try
            {
                var assignment = await _employeeAssignmentService.GetByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Employee assignment with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeAssignmentDto>.ErrorResult("An error occurred while retrieving the employee assignment", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee assignments by employee ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Employee assignment details</returns>
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeAssignmentDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeAssignmentDto>>>> GetEmployeeAssignmentsByEmployeeId(Guid employeeId)
        {
            try
            {
                var assignments = await _employeeAssignmentService.GetByEmployeeIdAsync(employeeId);
                return Ok(ApiResponse<IEnumerable<EmployeeAssignmentDto>>.SuccessResult(assignments, "Employee assignments retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeAssignmentDto>>.ErrorResult("An error occurred while retrieving employee assignments", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get active employee assignment by employee ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Active employee assignment details</returns>
        [HttpGet("employee/{employeeId}/active")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> GetActiveEmployeeAssignmentByEmployeeId(Guid employeeId)
        {
            try
            {
                var assignment = await _employeeAssignmentService.GetActiveByEmployeeIdAsync(employeeId);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Active employee assignment for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Active employee assignment retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeAssignmentDto>.ErrorResult("An error occurred while retrieving the active employee assignment", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new employee assignment
        /// </summary>
        /// <param name="createDto">Employee assignment creation data</param>
        /// <returns>Created employee assignment</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> CreateEmployeeAssignment(CreateEmployeeAssignmentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeAssignmentDto>.ErrorResult("Validation failed", errors));
                }

                var assignment = await _employeeAssignmentService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetEmployeeAssignment), new { id = assignment.Id },
                    ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeAssignmentDto>.ErrorResult("An error occurred while creating the employee assignment", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an employee assignment
        /// </summary>
        /// <param name="id">Employee Assignment ID</param>
        /// <param name="updateDto">Employee assignment update data</param>
        /// <returns>Updated employee assignment</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> UpdateEmployeeAssignment(Guid id, UpdateEmployeeAssignmentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeAssignmentDto>.ErrorResult("Validation failed", errors));
                }

                var assignment = await _employeeAssignmentService.UpdateAsync(id, updateDto);
                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeAssignmentDto>.ErrorResult("An error occurred while updating the employee assignment", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete an employee assignment
        /// </summary>
        /// <param name="id">Employee Assignment ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteEmployeeAssignment(Guid id)
        {
            try
            {
                await _employeeAssignmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the employee assignment", new List<string> { ex.Message }));
            }
        }
    }
}