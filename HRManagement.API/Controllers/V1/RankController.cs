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
    public class RankController(IRankService service) : ControllerBase
    {
        private readonly IRankService _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RankDto>>), 200)]
        public async Task<ActionResult<ApiResponse<PagedResult<RankDto>>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paged = await _service.GetPaged(pageNumber, pageSize);
            return Ok(ApiResponse<PagedResult<RankDto>>.SuccessResult(paged, "Ranks retrieved successfully"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RankDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<RankDto>>> GetById(long id)
        {
            var dto = await _service.GetById(id);
            if (dto == null)
                return NotFound(ApiResponse<RankDto>.ErrorResult($"Rank with ID {id} not found"));
            return Ok(ApiResponse<RankDto>.SuccessResult(dto));
        }

        [HttpGet("external/{guid}")]
        [ProducesResponseType(typeof(ApiResponse<RankDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<RankDto>>> GetByGuid(Guid guid)
        {
            var dto = await _service.GetByGuid(guid);
            if (dto == null)
                return NotFound(ApiResponse<RankDto>.ErrorResult($"Rank not found"));
            return Ok(ApiResponse<RankDto>.SuccessResult(dto));
        }

        [HttpGet("by-order/{order}")]
        [ProducesResponseType(typeof(ApiResponse<RankDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<RankDto>>> GetByOrder(int order)
        {
            var dto = await _service.GetByOrder(order);
            if (dto == null)
                return NotFound(ApiResponse<RankDto>.ErrorResult($"Rank with order {order} not found"));
            return Ok(ApiResponse<RankDto>.SuccessResult(dto));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RankDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<RankDto>>> Create(CreateRankDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<RankDto>.ErrorResult("Validation failed", errors));
            }
            try
            {
                var created = await _service.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<RankDto>.SuccessResult(created, "Rank created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<RankDto>.ErrorResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RankDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<RankDto>>> Update(long id, UpdateRankDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<RankDto>.ErrorResult("Validation failed", errors));
            }
            try
            {
                var updated = await _service.Update(id, dto);
                return Ok(ApiResponse<RankDto>.SuccessResult(updated, "Rank updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<RankDto>.ErrorResult(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse>> Delete(long id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
        }
    }
}



