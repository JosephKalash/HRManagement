using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Helpers;
using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeeProfilesController(IEmployeeProfileRepository employeeProfileRepository, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all employee profiles
        /// </summary>
        /// <returns>List of all employee profiles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeProfileDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeProfileDto>>>> GetEmployeeProfiles()
        {
            try
            {
                var profiles = await _employeeProfileRepository.GetAllAsync();
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var dtos = profiles.Select(profile =>
                {
                    var dto = _mapper.Map<EmployeeProfileDto>(profile);
                    dto.ImagePath = baseUrl + '/' + profile.ImagePath; // Call a method to modify the ImagePath
                    return dto;
                });
                return Ok(ApiResponse<IEnumerable<EmployeeProfileDto>>.SuccessResult(dtos, "Employee profiles retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeProfileDto>>.ErrorResult("An error occurred while retrieving employee profiles", new List<string> { ex.Message }));
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
                var profile = await _employeeProfileRepository.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(_mapper.Map<EmployeeProfileDto>(profile), "Employee profile retrieved successfully"));
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
                var profile = await _employeeProfileRepository.GetByEmployeeIdAsync(employeeId);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(_mapper.Map<EmployeeProfileDto>(profile), "Employee profile retrieved successfully"));
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

                var profile = _mapper.Map<EmployeeProfile>(createDto);

                var createdProfile = await _employeeProfileRepository.AddAsync(profile);
                return CreatedAtAction(nameof(GetEmployeeProfile), new { id = createdProfile.Id },
                    ApiResponse<EmployeeProfileDto>.SuccessResult(_mapper.Map<EmployeeProfileDto>(createdProfile), "Employee profile created successfully"));
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

                var profile = await _employeeProfileRepository.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile with ID {id} not found"));
                }

                UpdateProfileProperties(updateDto, profile);

                var updatedProfile = await _employeeProfileRepository.UpdateAsync(profile);
                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(_mapper.Map<EmployeeProfileDto>(updatedProfile), "Employee profile updated successfully"));
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
                var profile = await _employeeProfileRepository.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Employee profile with ID {id} not found"));
                }

                await _employeeProfileRepository.DeleteAsync(profile);
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

        private static void UpdateProfileProperties(UpdateEmployeeProfileDto updateDto, EmployeeProfile profile)
        {
            updateDto.Height.SetIfHasValue(val => profile.Height = val);
            updateDto.BloodGroup.SetIfHasValue(val => profile.BloodGroup = val);
            updateDto.SkinColor.SetIfNotNull(val => profile.SkinColor = val);
            updateDto.MobileNumber.SetIfHasValue(val => profile.MobileNumber = val);
            updateDto.HairColor.SetIfNotNull(val => profile.HairColor = val);
            updateDto.EyeColor.SetIfNotNull(val => profile.EyeColor = val);
            updateDto.DisabilityType.SetIfNotNull(val => profile.DisabilityType = val);
            updateDto.DistinctiveSigns.SetIfNotNull(val => profile.DistinctiveSigns = val);
            updateDto.CurrentNationality.SetIfNotNull(val => profile.CurrentNationality = val);
            updateDto.Religion.SetIfHasValue(val => profile.Religion = val);
            updateDto.PreviousNationality.SetIfNotNull(val => profile.PreviousNationality = val);
            updateDto.IssueNationalityDate.SetIfHasValue(val => profile.IssueNationalityDate = val);
            updateDto.SocialCondition.SetIfHasValue(val => profile.SocialCondition = val);
            updateDto.PlaceOfBirth.SetIfNotNull(val => profile.PlaceOfBirth = val);
            updateDto.InsuranceNumber.SetIfNotNull(val => profile.InsuranceNumber = val);
        }
    }
}