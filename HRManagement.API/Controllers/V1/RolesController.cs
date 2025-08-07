using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Enums;


// using HRManagement.Core.Enums;
using HRManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>List of all roles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResult(roles, "Roles retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<RoleDto>>.ErrorResult("An error occurred while retrieving roles", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>Role details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRole(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
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

        /// <summary>
        /// Get roles by level
        /// </summary>
        /// <param name="level">Role level</param>
        /// <returns>List of roles at the specified level</returns>
        [HttpGet("level/{level}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetRolesByLevel(RoleLevel level)
        {
            try
            {
                var roles = await _roleService.GetRolesByLevelAsync(level);
                return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResult(roles, $"Roles at level {level} retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<RoleDto>>.ErrorResult("An error occurred while retrieving roles by level", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="createDto">Role creation data</param>
        /// <returns>Created role</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
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

                var createdRole = await _roleService.CreateRoleAsync(createDto);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id },
                    ApiResponse<RoleDto>.SuccessResult(createdRole, "Role created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.ErrorResult("An error occurred while creating the role", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update a role
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <param name="updateDto">Role update data</param>
        /// <returns>Updated role</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
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

                var updatedRole = await _roleService.UpdateRoleAsync(id, updateDto);
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

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse>> DeleteRole(Guid id)
        {
            try
            {
                var deleted = await _roleService.DeleteRoleAsync(id);
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