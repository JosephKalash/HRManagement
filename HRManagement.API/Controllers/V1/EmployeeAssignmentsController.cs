using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeeAssignmentsController(IEmployeeAssignmentRepository employeeAssignmentRepository, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeAssignmentRepository _employeeAssignmentRepository = employeeAssignmentRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all employee assignments
        /// </summary>
        /// <returns>List of all employee assignments</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeAssignmentDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeAssignmentDto>>>> GetEmployeeAssignments()
        {
            try
            {
                var assignments = await _employeeAssignmentRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
                return Ok(ApiResponse<IEnumerable<EmployeeAssignmentDto>>.SuccessResult(dtos, "Employee assignments retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeAssignmentDto>>.ErrorResult("An error occurred while retrieving employee assignments", new List<string> { ex.Message }));
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
                var assignment = await _employeeAssignmentRepository.GetByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Employee assignment with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(_mapper.Map<EmployeeAssignmentDto>(assignment), "Employee assignment retrieved successfully"));
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
                var assignments = await _employeeAssignmentRepository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<EmployeeAssignmentDto>>(assignments);
                return Ok(ApiResponse<IEnumerable<EmployeeAssignmentDto>>.SuccessResult(dtos, "Employee assignments retrieved successfully"));
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
                var assignment = await _employeeAssignmentRepository.GetActiveByEmployeeIdAsync(employeeId);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Active employee assignment for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(_mapper.Map<EmployeeAssignmentDto>(assignment), "Active employee assignment retrieved successfully"));
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

                var assignment = _mapper.Map<EmployeeAssignment>(createDto);

                var createdAssignment = await _employeeAssignmentRepository.AddAsync(assignment);
                return CreatedAtAction(nameof(GetEmployeeAssignment), new { id = createdAssignment.Id }, 
                    ApiResponse<EmployeeAssignmentDto>.SuccessResult(_mapper.Map<EmployeeAssignmentDto>(createdAssignment), "Employee assignment created successfully"));
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

                var assignment = await _employeeAssignmentRepository.GetByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound(ApiResponse<EmployeeAssignmentDto>.ErrorResult($"Employee assignment with ID {id} not found"));
                }

                // Update properties
                if (updateDto.IsActive.HasValue)
                    assignment.IsActive = updateDto.IsActive.Value;
                if (updateDto.AssignedUnitId.HasValue)
                    assignment.AssignedUnitId = updateDto.AssignedUnitId.Value;
                if (updateDto.EndDate.HasValue)
                    assignment.EndDate = updateDto.EndDate.Value;
                if (updateDto.JobRoleId.HasValue)
                    assignment.JobRoleId = updateDto.JobRoleId.Value;
                if (updateDto.HiringDate.HasValue)
                    assignment.HiringDate = updateDto.HiringDate.Value;
                if (updateDto.GrantingAuthority.HasValue)
                    assignment.GrantingAuthority = updateDto.GrantingAuthority.Value;
                if (updateDto.LastPromotion.HasValue)
                    assignment.LastPromotion = updateDto.LastPromotion.Value;
                if (updateDto.ContractDuration.HasValue)
                    assignment.ContractDuration = updateDto.ContractDuration.Value;
                if (updateDto.ServiceDuration.HasValue)
                    assignment.ServiceDuration = updateDto.ServiceDuration.Value;
                if (updateDto.AssignmentType.HasValue)
                    assignment.AssignmentType = updateDto.AssignmentType.Value;
                if (updateDto.Name != null)
                    assignment.Name = updateDto.Name;
                if (updateDto.Description != null)
                    assignment.Description = updateDto.Description;

                var updatedAssignment = await _employeeAssignmentRepository.UpdateAsync(assignment);
                return Ok(ApiResponse<EmployeeAssignmentDto>.SuccessResult(_mapper.Map<EmployeeAssignmentDto>(updatedAssignment), "Employee assignment updated successfully"));
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
                var assignment = await _employeeAssignmentRepository.GetByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Employee assignment with ID {id} not found"));
                }

                await _employeeAssignmentRepository.DeleteAsync(assignment);
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