using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class EmployeesRankController(IEmployeeRankService employeeRankService) : ControllerBase
    {
        private readonly IEmployeeRankService _employeeRankService = employeeRankService;


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeRankDto>>), 200)]
        public async Task<ActionResult<ApiResponse<List<EmployeeRankDto>>>> GetAll()
        {
            try
            {
                var ranks = await _employeeRankService.GetAll();
                return ApiResponse<List<EmployeeRankDto>>.SuccessResult(ranks);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<List<EmployeeRankDto>>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeRankDto?>>> GetByIdEmployeeRank(long id)
        {
            try
            {
                var rank = await _employeeRankService.GetById(id);
                if (rank == null)
                    return ApiResponse<EmployeeRankDto?>.ErrorResult("EmployeeRank not found", ["EmployeeRank with the specified ID does not exist."]);

                return ApiResponse<EmployeeRankDto?>.SuccessResult(rank);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<EmployeeRankDto?>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpGet("ByEmployee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeRankDto>>>> GetByEmployeeId(long employeeId)
        {
            try
            {
                var ranks = await _employeeRankService.GetByEmployeeId(employeeId);
                return ApiResponse<List<EmployeeRankDto>>.SuccessResult(ranks);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<List<EmployeeRankDto>>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpGet("ByRank/{rankId}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeRankDto>>>> GetByRankId(long rankId)
        {
            try
            {
                var ranks = await _employeeRankService.GetByRankId(rankId);
                return ApiResponse<List<EmployeeRankDto>>.SuccessResult(ranks);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<List<EmployeeRankDto>>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmployeeRankDto>>> Create([FromBody] CreateEmployeeRankDto createDto)
        {
            try
            {
                var createdRank = await _employeeRankService.Create(createDto);
                return ApiResponse<EmployeeRankDto>.SuccessResult(createdRank, "EmployeeRank created successfully");
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<EmployeeRankDto>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeRankDto>>> Update(long id, [FromBody] UpdateEmployeeRankDto updateDto)
        {
            try
            {
                var updatedRank = await _employeeRankService.Update(id, updateDto);
                return ApiResponse<EmployeeRankDto>.SuccessResult(updatedRank, "EmployeeRank updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return ApiResponse<EmployeeRankDto>.ErrorResult(ex.Message, [ex.Message]);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<EmployeeRankDto>.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(long id)
        {
            try
            {
                await _employeeRankService.Delete(id);
                return ApiResponse.SuccessResult("EmployeeRank deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return ApiResponse.ErrorResult(ex.Message, [ex.Message]);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse.ErrorResult(ex.Message, [ex.Message]);
            }
        }

        [HttpGet("Exists/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> ActiveExists(long id)
        {
            try
            {
                var exists = await _employeeRankService.ActiveExists(id);
                return ApiResponse<bool>.SuccessResult(exists);
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<bool>.ErrorResult(ex.Message, [ex.Message]);
            }
        }
    }
}