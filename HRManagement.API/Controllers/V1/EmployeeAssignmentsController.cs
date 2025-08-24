using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

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

        
        
        
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeAssignmentDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeAssignmentDto>>>> GetEmployeeAssignments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeAssignmentService.GetPaged(pageNumber, pageSize);
                var dtos = _mapper.Map<List<EmployeeAssignmentDto>>(pagedResult.Items);
                return Ok(ApiResponse<PagedResult<EmployeeAssignmentDto>>.SuccessResult(new PagedResult<EmployeeAssignmentDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee assignments retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<PagedResult<EmployeeAssignmentDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> GetEmployeeAssignment(Guid id)
        {
            try
            {
                var assignment = await _employeeAssignmentService.GetById(id);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Employee assignment with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeAssignmentDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeAssignmentDto>>>> GetEmployeeAssignmentsByEmployeeId(Guid employeeId)
        {
            try
            {
                var assignments = await _employeeAssignmentService.GetByEmployeeId(employeeId);
                return Ok(ApiResponse<IEnumerable<EmployeeAssignmentDto>>.SuccessResult(assignments, "Employee assignments retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<IEnumerable<EmployeeAssignmentDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        [HttpGet("employee/{employeeId}/active")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeAssignmentDto>>> GetActiveEmployeeAssignmentByEmployeeId(Guid employeeId)
        {
            try
            {
                var assignment = await _employeeAssignmentService.GetActiveByEmployeeId(employeeId);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Active employee assignment for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Active employee assignment retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

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

                var assignment = await _employeeAssignmentService.Create(createDto);
                return CreatedAtAction(nameof(GetEmployeeAssignment), new { id = assignment.Id },
                    ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeAssignmentDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

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

                var assignment = await _employeeAssignmentService.Update(id, updateDto);
                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(assignment, "Employee assignment updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<EmployeeAssignmentDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        
        
        
        
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteEmployeeAssignment(Guid id)
        {
            try
            {
                await _employeeAssignmentService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }
    }
}