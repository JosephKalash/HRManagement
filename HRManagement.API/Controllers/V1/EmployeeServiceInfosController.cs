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
    public class EmployeeServiceInfosController : ControllerBase
    {
        private readonly IEmployeeServiceInfoService _employeeServiceInfoService;
        private readonly IMapper _mapper;

        public EmployeeServiceInfosController(IEmployeeServiceInfoService employeeServiceInfoService, IMapper mapper)
        {
            _employeeServiceInfoService = employeeServiceInfoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employee service infos
        /// </summary>
        /// <returns>List of all employee service infos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeServiceInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeServiceInfoService.GetPagedAsync(pageNumber, pageSize);
                var dtos = _mapper.Map<List<EmployeeServiceInfoDto>>(pagedResult.Items);
                return Ok(ApiResponse<PagedResult<EmployeeServiceInfoDto>>.SuccessResult(new PagedResult<EmployeeServiceInfoDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee service infos retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeServiceInfoDto>>.ErrorResult("An error occurred while retrieving employee service infos", new List<string> { ex.Message }));
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
                var serviceInfo = await _employeeServiceInfoService.GetByIdAsync(id);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Employee service info with ID {id} not found"));
                }

                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(serviceInfo, "Employee service info retrieved successfully"));
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
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeServiceInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfosByEmployeeId(Guid employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeServiceInfoService.GetPagedByEmployeeIdAsync(employeeId, pageNumber, pageSize);
                var dtos = _mapper.Map<List<EmployeeServiceInfoDto>>(pagedResult.Items);
                return Ok(ApiResponse<PagedResult<EmployeeServiceInfoDto>>.SuccessResult(new PagedResult<EmployeeServiceInfoDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee service infos retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeServiceInfoDto>>.ErrorResult("An error occurred while retrieving employee service infos", new List<string> { ex.Message }));
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
                var serviceInfo = await _employeeServiceInfoService.GetActiveByEmployeeIdAsync(employeeId);
                if (serviceInfo == null)
                {
                    return NotFound(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Active employee service info for employee ID {employeeId} not found"));
                }

                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(serviceInfo, "Active employee service info retrieved successfully"));
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

                var serviceInfo = await _employeeServiceInfoService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetEmployeeServiceInfo), new { id = serviceInfo.Id },
                    ApiResponse<EmployeeServiceInfoDto>.SuccessResult(serviceInfo, "Employee service info created successfully"));
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

                var serviceInfo = await _employeeServiceInfoService.UpdateAsync(id, updateDto);
                return Ok(ApiResponse<EmployeeServiceInfoDto>.SuccessResult(serviceInfo, "Employee service info updated successfully"));
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
                await _employeeServiceInfoService.DeleteAsync(id);
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