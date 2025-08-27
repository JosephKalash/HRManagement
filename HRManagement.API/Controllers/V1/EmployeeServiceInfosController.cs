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
    public class EmployeeServiceInfosController : ControllerBase
    {
        private readonly IEmployeeServiceInfoService _employeeServiceInfoService;
        private readonly IMapper _mapper;

        public EmployeeServiceInfosController(IEmployeeServiceInfoService employeeServiceInfoService, IMapper mapper)
        {
            _employeeServiceInfoService = employeeServiceInfoService;
            _mapper = mapper;
        }





        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeServiceInfoDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeServiceInfoService.GetPaged(pageNumber, pageSize);
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






        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> GetEmployeeServiceInfo(long id)
        {
            try
            {
                var serviceInfo = await _employeeServiceInfoService.GetById(id);
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






        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeServiceInfoDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeServiceInfoDto>>>> GetEmployeeServiceInfosByEmployeeId(long employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeServiceInfoService.GetPagedByEmployeeId(employeeId, pageNumber, pageSize);
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






        [HttpGet("employee/{employeeId}/active")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> GetActiveEmployeeServiceInfoByEmployeeId(long employeeId)
        {
            try
            {
                var serviceInfo = await _employeeServiceInfoService.GetActiveByEmployeeId(employeeId);
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






        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

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

                var service = await _employeeServiceInfoService.GetActiveByEmployeeId(createDto.EmployeeId);
                if (service != null)
                {
                    return BadRequest(ApiResponse<EmployeeServiceInfoDto>.ErrorResult($"Employee has already an active service"));
                }

                var serviceInfo = await _employeeServiceInfoService.Create(createDto);
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







        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeServiceInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<EmployeeServiceInfoDto>>> UpdateEmployeeServiceInfo(long id, UpdateEmployeeServiceInfoDto updateDto)
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

                var serviceInfo = await _employeeServiceInfoService.Update(id, updateDto);
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






        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteEmployeeServiceInfo(long id)
        {
            try
            {
                await _employeeServiceInfoService.Delete(id);
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