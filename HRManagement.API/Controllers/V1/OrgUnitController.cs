using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class OrgUnitController : ControllerBase
    {
        private readonly IOrgUnitService _orgUnitService;

        public OrgUnitController(IOrgUnitService orgUnitService)
        {
            _orgUnitService = orgUnitService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<OrgUnitDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<OrgUnitDto>>>> GetOrgUnits([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _orgUnitService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<OrgUnitDto>>.SuccessResult(pagedResult, "Org units retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<PagedResult<OrgUnitDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<OrgUnitDto>>> GetOrgUnit(Guid id)
        {
            try
            {
                var orgUnit = await _orgUnitService.GetByIdAsync(id);
                if (orgUnit == null)
                {
                    return NotFound(ApiResponse<OrgUnitDto>.ErrorResult($"OrgUnit with ID {id} not found"));
                }
                return Ok(ApiResponse<OrgUnitDto>.SuccessResult(orgUnit, "Org unit retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<OrgUnitDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpGet("parent/{parentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrgUnitDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<OrgUnitDto>>>> GetByParentId(Guid? parentId)
        {
            try
            {
                var orgUnits = await _orgUnitService.GetByParentIdAsync(parentId);
                return Ok(ApiResponse<IEnumerable<OrgUnitDto>>.SuccessResult(orgUnits, "Org units by parent retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<IEnumerable<OrgUnitDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpGet("type/{type}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrgUnitDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<OrgUnitDto>>>> GetByType(OrgUnitType type)
        {
            try
            {
                var orgUnits = await _orgUnitService.GetByTypeAsync(type);
                return Ok(ApiResponse<IEnumerable<OrgUnitDto>>.SuccessResult(orgUnits, "Org units by type retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<IEnumerable<OrgUnitDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrgUnitDto>>), 200)]

        public async Task<ActionResult<ApiResponse<IEnumerable<OrgUnitDto>>>> SearchByName([FromQuery] string searchTerm)
        {
            try
            {
                var orgUnits = await _orgUnitService.SearchByNameAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<OrgUnitDto>>.SuccessResult(orgUnits, $"Search results for '{searchTerm}' retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<IEnumerable<OrgUnitDto>>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<ApiResponse<OrgUnitDto>>> CreateOrgUnit(CreateOrgUnitDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<OrgUnitDto>.ErrorResult("Validation failed", errors));
                }
                var orgUnit = await _orgUnitService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetOrgUnit), new { id = orgUnit.Id }, ApiResponse<OrgUnitDto>.SuccessResult(orgUnit, "Org unit created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<OrgUnitDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<OrgUnitDto>.ErrorResult(ex.Message, [ex.Message]));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<OrgUnitDto>>> UpdateOrgUnit(Guid id, UpdateOrgUnitDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(ApiResponse<OrgUnitDto>.ErrorResult("Validation failed", errors));
                }
                var orgUnit = await _orgUnitService.UpdateAsync(id, dto);
                return Ok(ApiResponse<OrgUnitDto>.SuccessResult(orgUnit, "Org unit updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<OrgUnitDto>.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<OrgUnitDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteOrgUnit(Guid id)
        {
            try
            {
                await _orgUnitService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }

        [HttpGet("hierarchy")]
        [OutputCache(PolicyName = "OrgHierarchy")]
        [ProducesResponseType(typeof(ApiResponse<OrgUnitHierarchyDto>), 200)]

        public async Task<ActionResult<ApiResponse<OrgUnitHierarchyDto>>> GetHierarchy()
        {
            try
            {
                var hierarchy = await _orgUnitService.GetHierarchyAsync();
                return Ok(ApiResponse<OrgUnitHierarchyDto>.SuccessResult(hierarchy, "Organization hierarchy retrieved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ApiResponse<OrgUnitHierarchyDto>.ErrorResult(ex.Message, new List<string> { ex.Message }));
            }
        }
    }
}