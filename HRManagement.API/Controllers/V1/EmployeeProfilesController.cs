using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Helpers;
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
    public class EmployeeProfilesController(IEmployeeProfileService employeeProfileService, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeProfileService _employeeProfileService = employeeProfileService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all employee profiles
        /// </summary>
        /// <returns>List of all employee profiles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeProfileDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeProfileDto>>>> GetEmployeeProfiles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeProfileService.GetPagedAsync(pageNumber, pageSize);
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var dtos = pagedResult.Items.Select(profile =>
                {
                    var dto = _mapper.Map<EmployeeProfileDto>(profile);
                    if (profile.ImagePath != null)
                    {
                        dto.ImagePath = baseUrl + '/' + profile.ImagePath;
                    }
                    return dto;
                }).ToList();
                return Ok(ApiResponse<PagedResult<EmployeeProfileDto>>.SuccessResult(new PagedResult<EmployeeProfileDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee profiles retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeProfileDto>>.ErrorResult("An error occurred while retrieving employee profiles", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee profile by ID
        /// </summary>
        /// <param name="id">Employee Profile ID</param>
        /// <returns>Employee profile details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> GetEmployeeProfile(Guid id)
        {
            try
            {
                var profile = await _employeeProfileService.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(profile, "Employee profile retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeProfileDto>.ErrorResult("An error occurred while retrieving the employee profile", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee profile by employee ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Employee profile details</returns>
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> GetEmployeeProfileByEmployeeId(Guid employeeId)
        {
            try
            {
                var profile = await _employeeProfileService.GetByEmployeeIdAsync(employeeId);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(profile, "Employee profile retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeProfileDto>.ErrorResult("An error occurred while retrieving the employee profile", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new employee profile
        /// </summary>
        /// <param name="createDto">Employee profile creation data</param>
        /// <returns>Created employee profile</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> CreateEmployeeProfile(CreateEmployeeProfileDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Validation failed", errors));
                }

                var profile = await _employeeProfileService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetEmployeeProfile), new { id = profile.Id },
                    ApiResponse<EmployeeProfileDto>.SuccessResult(profile, "Employee profile created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeProfileDto>.ErrorResult("An error occurred while creating the employee profile", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an employee profile
        /// </summary>
        /// <param name="id">Employee Profile ID</param>
        /// <param name="updateDto">Employee profile update data</param>
        /// <returns>Updated employee profile</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> UpdateEmployeeProfile(Guid id, UpdateEmployeeProfileDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Validation failed", errors));
                }

                var profile = await _employeeProfileService.UpdateAsync(id, updateDto);
                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(profile, "Employee profile updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeProfileDto>.ErrorResult("An error occurred while updating the employee profile", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete an employee profile
        /// </summary>
        /// <param name="id">Employee Profile ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteEmployeeProfile(Guid id)
        {
            try
            {
                await _employeeProfileService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the employee profile", new List<string> { ex.Message }));
            }
        }
    }
}