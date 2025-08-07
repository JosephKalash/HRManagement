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
    public class EmployeeServiceInfosController : ControllerBase
    {
        private readonly IEmployeeServiceInfoRepository _employeeServiceInfoRepository;
        private readonly IMapper _mapper;

        public EmployeeServiceInfosController(IEmployeeServiceInfoRepository employeeServiceInfoRepository, IMapper mapper)
        {
            _employeeServiceInfoRepository = employeeServiceInfoRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employee service infos
        /// </summary>
        /// <returns>List of all employee service infos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeServiceInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfos()
        {
            try
            {
                var serviceInfos = await _employeeServiceInfoRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
                return Ok(ApiResponse<IEnumerable<EmployeeServiceInfoDto>>.SuccessResult(dtos, "Employee service infos retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeServiceInfoDto>>.ErrorResult("An error occurred while retrieving employee service infos", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee service info by ID
        /// </summary>
        /// <param name="id">Employee Service Info ID</param>
        /// <returns>Employee service info details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> GetEmployeeServiceInfo(Guid id)
        {
            try
            {
                var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Employee service info with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(_mapper.Map<EmployeeServiceInfoDto>(serviceInfo), "Employee service info retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeServiceInfoDto>.ErrorResult("An error occurred while retrieving the employee service info", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get employee service infos by employee ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Employee service info details</returns>
        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeServiceInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfosByEmployeeId(Guid employeeId)
        {
            try
            {
                var serviceInfos = await _employeeServiceInfoRepository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
                return Ok(ApiResponse<IEnumerable<EmployeeServiceInfoDto>>.SuccessResult(dtos, "Employee service infos retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<EmployeeServiceInfoDto>>.ErrorResult("An error occurred while retrieving employee service infos", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get active employee service info by employee ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Active employee service info details</returns>
        [HttpGet("employee/{employeeId}/active")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> GetActiveEmployeeServiceInfoByEmployeeId(Guid employeeId)
        {
            try
            {
                var serviceInfo = await _employeeServiceInfoRepository.GetActiveByEmployeeIdAsync(employeeId);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Active employee service info for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(_mapper.Map<EmployeeServiceInfoDto>(serviceInfo), "Active employee service info retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeServiceInfoDto>.ErrorResult("An error occurred while retrieving the active employee service info", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new employee service info
        /// </summary>
        /// <param name="createDto">Employee service info creation data</param>
        /// <returns>Created employee service info</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> CreateEmployeeServiceInfo(CreateEmployeeServiceInfoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeServiceInfoDto>.ErrorResult("Validation failed", errors));
                }

                var serviceInfo = _mapper.Map<EmployeeServiceInfo>(createDto);

                var createdServiceInfo = await _employeeServiceInfoRepository.AddAsync(serviceInfo);
                return CreatedAtAction(nameof(GetEmployeeServiceInfo), new { id = createdServiceInfo.Id },
                    ApiResponse<EmployeeServiceInfoDto>.SuccessResult(_mapper.Map<EmployeeServiceInfoDto>(createdServiceInfo), "Employee service info created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<EmployeeServiceInfoDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeServiceInfoDto>.ErrorResult("An error occurred while creating the employee service info", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an employee service info
        /// </summary>
        /// <param name="id">Employee Service Info ID</param>
        /// <param name="updateDto">Employee service info update data</param>
        /// <returns>Updated employee service info</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> UpdateEmployeeServiceInfo(Guid id, UpdateEmployeeServiceInfoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<EmployeeServiceInfoDto>.ErrorResult("Validation failed", errors));
                }

                var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Employee service info with ID {id} not found"));
                }

                // Update properties
                if (updateDto.BelongingUnitId.HasValue)
                    serviceInfo.BelongingUnitId = updateDto.BelongingUnitId.Value;
                if (updateDto.Ownership.HasValue)
                    serviceInfo.Ownership = updateDto.Ownership.Value;
                if (updateDto.JobRoleId.HasValue)
                    serviceInfo.JobRoleId = updateDto.JobRoleId.Value;
                if (updateDto.HiringDate.HasValue)
                    serviceInfo.HiringDate = updateDto.HiringDate.Value;
                if (updateDto.GrantingAuthority.HasValue)
                    serviceInfo.GrantingAuthority = updateDto.GrantingAuthority.Value;
                if (updateDto.LastPromotion.HasValue)
                    serviceInfo.LastPromotion = updateDto.LastPromotion.Value;
                if (updateDto.ContractDuration.HasValue)
                    serviceInfo.ContractDuration = updateDto.ContractDuration.Value;
                if (updateDto.ServiceDuration.HasValue)
                    serviceInfo.ServiceDuration = updateDto.ServiceDuration.Value;
                if (updateDto.BaseSalary.HasValue)
                    serviceInfo.BaseSalary = updateDto.BaseSalary.Value;
                if (updateDto.IsMilitaryCoach.HasValue)
                    serviceInfo.IsMilitaryCoach = updateDto.IsMilitaryCoach.Value;
                if (updateDto.IsDeductedMinistryVacancies.HasValue)
                    serviceInfo.IsDeductedMinistryVacancies = updateDto.IsDeductedMinistryVacancies.Value;
                if (updateDto.IsRetiredFederalMinistry.HasValue)
                    serviceInfo.IsRetiredFederalMinistry = updateDto.IsRetiredFederalMinistry.Value;
                if (updateDto.IsNationalService.HasValue)
                    serviceInfo.IsNationalService = updateDto.IsNationalService.Value;
                if (updateDto.ProfessionalSupport.HasValue)
                    serviceInfo.ProfessionalSupport = updateDto.ProfessionalSupport.Value;
                if (updateDto.EffectiveDate.HasValue)
                    serviceInfo.EffectiveDate = updateDto.EffectiveDate.Value;
                if (updateDto.EndDate.HasValue)
                    serviceInfo.EndDate = updateDto.EndDate.Value;
                if (updateDto.IsActive.HasValue)
                    serviceInfo.IsActive = updateDto.IsActive.Value;

                var updatedServiceInfo = await _employeeServiceInfoRepository.UpdateAsync(serviceInfo);
                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(_mapper.Map<EmployeeServiceInfoDto>(updatedServiceInfo), "Employee service info updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<EmployeeServiceInfoDto>.ErrorResult("An error occurred while updating the employee service info", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete an employee service info
        /// </summary>
        /// <param name="id">Employee Service Info ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteEmployeeServiceInfo(Guid id)
        {
            try
            {
                var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Employee service info with ID {id} not found"));
                }

                await _employeeServiceInfoRepository.DeleteAsync(serviceInfo);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the employee service info", new List<string> { ex.Message }));
            }
        }
    }
}