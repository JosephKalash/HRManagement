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
    public class OrgUnitProfileController(IOrgUnitProfileService service) : ControllerBase
    {
        private readonly IOrgUnitProfileService _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<OrgUnitProfileDto>>), 200)]
        public async Task<ActionResult<ApiResponse<PagedResult<OrgUnitProfileDto>>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paged = await _service.GetPaged(pageNumber, pageSize);
            return Ok(ApiResponse<PagedResult<OrgUnitProfileDto>>.SuccessResult(paged, "Profiles retrieved successfully"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<OrgUnitProfileDto>>> GetById(long id)
        {
            var dto = await _service.GetById(id);
            if (dto == null)
                return NotFound(ApiResponse<OrgUnitProfileDto>.ErrorResult($"Profile with ID {id} not found"));
            return Ok(ApiResponse<OrgUnitProfileDto>.SuccessResult(dto));
        }

        [HttpGet("by-org/{orgUnitId}")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<OrgUnitProfileDto>>> GetByOrgUnitId(long orgUnitId)
        {
            var dto = await _service.GetByOrgUnitId(orgUnitId);
            if (dto == null)
                return NotFound(ApiResponse<OrgUnitProfileDto>.ErrorResult($"Profile for OrgUnit {orgUnitId} not found"));
            return Ok(ApiResponse<OrgUnitProfileDto>.SuccessResult(dto));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitProfileDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse<OrgUnitProfileDto>>> Create(CreateOrgUnitProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<OrgUnitProfileDto>.ErrorResult("Validation failed", errors));
            }
            try
            {
                var created = await _service.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<OrgUnitProfileDto>.SuccessResult(created, "Profile created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<OrgUnitProfileDto>.ErrorResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ApiResponse<OrgUnitProfileDto>>> Update(long id, UpdateOrgUnitProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<OrgUnitProfileDto>.ErrorResult("Validation failed", errors));
            }
            try
            {
                var updated = await _service.Update(id, dto);
                return Ok(ApiResponse<OrgUnitProfileDto>.SuccessResult(updated, "Profile updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<OrgUnitProfileDto>.ErrorResult(ex.Message));
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


