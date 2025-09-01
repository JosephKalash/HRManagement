using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;


// using HRManagement.Core.Enums;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }





        [HttpGet]
        [OutputCache(PolicyName = "RolesPaged")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RoleDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<RoleDto>>>> GetRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _roleService.GetPagedRoles(pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<RoleDto>>.SuccessResult(pagedResult, "Roles retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRole(long id)
        {
            try
            {
                var role = await _roleService.GetRoleById(id);
                if (role == null)
                {
                    return NotFound(ApiResponse<RoleDto>.ErrorResult($"Role with ID {id} not found"));
                }

                return Ok(ApiResponse<RoleDto>.SuccessResult(role, "Role retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpGet("level/{level}")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RoleDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<RoleDto>>>> GetRolesByLevel(RoleLevel level, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _roleService.GetPagedRolesByLevel(level, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<RoleDto>>.SuccessResult(pagedResult, $"Roles at level {level} retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }
        [HttpGet("external/{guid}")]
        [OutputCache(PolicyName = "RoleByGuid")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleByGuid(Guid guid)
        {
            try
            {
                var role = await _roleService.GetRoleByGuid(guid);
                if (role == null)
                {
                    return NotFound(ApiResponse<RoleDto>.ErrorResult($"Role not found"));
                }

                return Ok(ApiResponse<RoleDto>.SuccessResult(role, "Role retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message, [ex.Message]));
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] CreateRoleDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<RoleDto>.ErrorResult("Validation failed", errors));
                }

                var createdRole = await _roleService.Create(createDto);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id },
                    ApiResponse<RoleDto>.SuccessResult(createdRole, "Role created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<RoleDto>.ErrorResult(ex.Message));
            }
        }






        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(long id, [FromBody] UpdateRoleDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<RoleDto>.ErrorResult("Validation failed", errors));
                }

                var updatedRole = await _roleService.UpdateRole(id, updateDto);
                if (updatedRole == null)
                {
                    return NotFound(ApiResponse<RoleDto>.ErrorResult($"Role with ID {id} not found"));
                }

                return Ok(ApiResponse<RoleDto>.SuccessResult(updatedRole, "Role updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<RoleDto>.ErrorResult(ex.Message));
            }
        }






        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteRole(long id)
        {
            try
            {
                var deleted = await _roleService.DeleteRole(id);
                if (!deleted)
                {
                    return NotFound(ApiResponse.ErrorResult($"Role with ID {id} not found"));
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<RoleDto>.ErrorResult(ex.Message));
            }
        }
    }
}