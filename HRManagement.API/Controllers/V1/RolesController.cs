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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<RoleDto>>.ErrorResult("An error occurred while retrieving roles", new List<string> { ex.Message }));
            }
        }






        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRole(Guid id)
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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.ErrorResult("An error occurred while retrieving the role", new List<string> { ex.Message }));
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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<RoleDto>>.ErrorResult("An error occurred while retrieving roles by level", new List<string> { ex.Message }));
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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.ErrorResult("An error occurred while creating the role", new List<string> { ex.Message }));
            }
        }







        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(Guid id, [FromBody] UpdateRoleDto updateDto)
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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.ErrorResult("An error occurred while updating the role", new List<string> { ex.Message }));
            }
        }






        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]

        public async Task<ActionResult<ApiResponse>> DeleteRole(Guid id)
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
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the role", new List<string> { ex.Message }));
            }
        }
    }
}