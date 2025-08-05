using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
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
    public class EmployeeProfilesController : ControllerBase
    {
        private readonly IEmployeeProfileRepository _employeeProfileRepository;

        public EmployeeProfilesController(IEmployeeProfileRepository employeeProfileRepository)
        {
            _employeeProfileRepository = employeeProfileRepository;
        }

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
                var dtos = profiles.Select(MapToDto);
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

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(MapToDto(profile), "Employee profile retrieved successfully"));
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

                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(MapToDto(profile), "Employee profile retrieved successfully"));
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

                var profile = new EmployeeProfile
                {
                    EmployeeId = createDto.EmployeeId,
                    Height = createDto.Height,
                    BloodGroup = createDto.BloodGroup,
                    SkinColor = createDto.SkinColor,
                    HairColor = createDto.HairColor,
                    EyeColor = createDto.EyeColor,
                    DisabilityType = createDto.DisabilityType,
                    DistinctiveSigns = createDto.DistinctiveSigns,
                    CurrentNationality = createDto.CurrentNationality,
                    Religion = createDto.Religion,
                    PreviousNationality = createDto.PreviousNationality,
                    IssueNationalityDate = createDto.IssueNationalityDate,
                    SocialCondition = createDto.SocialCondition,
                    PlaceOfBirth = createDto.PlaceOfBirth,
                    InsuranceNumber = createDto.InsuranceNumber
                };

                var createdProfile = await _employeeProfileRepository.AddAsync(profile);
                return CreatedAtAction(nameof(GetEmployeeProfile), new { id = createdProfile.Id }, 
                    ApiResponse<EmployeeProfileDto>.SuccessResult(MapToDto(createdProfile), "Employee profile created successfully"));
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
        [HttpPut("{id}")]
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
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeProfileDto>.ErrorResult("Validation failed", errors));
                }

                var profile = await _employeeProfileRepository.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(ApiResponse<EmployeeProfileDto>.ErrorResult($"Employee profile with ID {id} not found"));
                }

                // Update properties
                if (updateDto.Height.HasValue)
                    profile.Height = updateDto.Height.Value;
                if (updateDto.BloodGroup.HasValue)
                    profile.BloodGroup = updateDto.BloodGroup.Value;
                if (updateDto.SkinColor != null)
                    profile.SkinColor = updateDto.SkinColor;
                if (updateDto.HairColor != null)
                    profile.HairColor = updateDto.HairColor;
                if (updateDto.EyeColor != null)
                    profile.EyeColor = updateDto.EyeColor;
                if (updateDto.DisabilityType != null)
                    profile.DisabilityType = updateDto.DisabilityType;
                if (updateDto.DistinctiveSigns != null)
                    profile.DistinctiveSigns = updateDto.DistinctiveSigns;
                if (updateDto.CurrentNationality != null)
                    profile.CurrentNationality = updateDto.CurrentNationality;
                if (updateDto.Religion != null)
                    profile.Religion = updateDto.Religion;
                if (updateDto.PreviousNationality.HasValue)
                    profile.PreviousNationality = updateDto.PreviousNationality.Value;
                if (updateDto.IssueNationalityDate.HasValue)
                    profile.IssueNationalityDate = updateDto.IssueNationalityDate.Value;
                if (updateDto.SocialCondition.HasValue)
                    profile.SocialCondition = updateDto.SocialCondition.Value;
                if (updateDto.PlaceOfBirth != null)
                    profile.PlaceOfBirth = updateDto.PlaceOfBirth;
                if (updateDto.InsuranceNumber != null)
                    profile.InsuranceNumber = updateDto.InsuranceNumber;

                var updatedProfile = await _employeeProfileRepository.UpdateAsync(profile);
                return Ok(ApiResponse<EmployeeProfileDto>.SuccessResult(MapToDto(updatedProfile), "Employee profile updated successfully"));
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

        private static EmployeeProfileDto MapToDto(EmployeeProfile profile)
        {
            return new EmployeeProfileDto
            {
                Id = profile.Id,
                EmployeeId = profile.EmployeeId,
                Height = profile.Height,
                BloodGroup = profile.BloodGroup,
                SkinColor = profile.SkinColor,
                HairColor = profile.HairColor,
                EyeColor = profile.EyeColor,
                DisabilityType = profile.DisabilityType,
                DistinctiveSigns = profile.DistinctiveSigns,
                CurrentNationality = profile.CurrentNationality,
                Religion = profile.Religion,
                PreviousNationality = profile.PreviousNationality,
                IssueNationalityDate = profile.IssueNationalityDate,
                SocialCondition = profile.SocialCondition,
                PlaceOfBirth = profile.PlaceOfBirth,
                InsuranceNumber = profile.InsuranceNumber
            };
        }
    }
} 