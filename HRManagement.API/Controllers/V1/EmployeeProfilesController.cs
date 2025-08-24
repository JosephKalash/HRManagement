using System.Text.Json;
using AutoMapper;
using HRManagement.API.Models;
using HRManagement.Application.DTOs;
using HRManagement.Application.Helpers;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Http;
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

        
        
        
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeProfileDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeProfileDto>>>> GetEmployeeProfiles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeProfileService.GetPaged(pageNumber, pageSize);
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var dtos = pagedResult.Items.Select(profile =>
                {
                    var dto = _mapper.Map<EmployeeProfileDto>(profile);
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

        
        
        
        
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> GetEmployeeProfile(Guid id)
        {
            try
            {
                var profile = await _employeeProfileService.GetById(id);
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

        
        
        
        
        
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> GetEmployeeProfileByEmployeeId(Guid employeeId)
        {
            try
            {
                var profile = await _employeeProfileService.GetByEmployeeId(employeeId);
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

        
        
        
        
        
        
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> CreateEmployeeProfile([FromForm] CreateEmployeeProfileRequest profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Validation failed", errors));
                }

                var createDto = _mapper.Map<CreateEmployeeProfileDto>(profile);


                if (createDto == null)
                {
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Invalid profile data"));
                }


                using var stream = profile.Image?.OpenReadStream();
                var createdProfile = await _employeeProfileService.Create(createDto, stream, profile.Image?.FileName);


                return CreatedAtAction(nameof(GetEmployeeProfile), new { id = createdProfile.Id },
                    ApiResponse<EmployeeProfileDto>.SuccessResult(createdProfile, "Employee profile created successfully"));
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

        
        
        
        
        
        
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeProfileDto>>> UpdateEmployeeProfile(Guid id, [FromForm] UpdateEmployeeProfileRequest updateRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Validation failed", errors));
                }

                var updateDto = _mapper.Map<UpdateEmployeeProfileDto>(updateRequest);


                if (updateDto == null)
                {
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Invalid profile data"));
                }
                using var stream = updateRequest.Image?.OpenReadStream();
                var profile = await _employeeProfileService.Update(id, updateDto, stream, updateRequest.Image?.FileName);
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

        
        
        
        
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteEmployeeProfile(Guid id)
        {
            try
            {
                await _employeeProfileService.Delete(id);
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