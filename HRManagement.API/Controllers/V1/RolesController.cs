using HRManagement.Application.DTOs;
using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;
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
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
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
                var roles = await _roleRepository.GetAllAsync();
                var dtos = roles.Select(MapToDto);
                return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResult(dtos, "Roles retrieved successfully"));
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
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(ApiResponse<RoleDto>.ErrorResult($"Role with ID {id} not found"));
                }

                return Ok(ApiResponse<RoleDto>.SuccessResult(MapToDto(role), "Role retrieved successfully"));
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
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetRolesByLevel(int level)
        {
            try
            {
                var roles = await _roleRepository.GetByLevelAsync(level);
                var dtos = roles.Select(MapToDto);
                return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResult(dtos, $"Roles at level {level} retrieved successfully"));
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
        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole(CreateRoleDto createDto)
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

                var role = new Role
                {
                    Level = createDto.Level,
                    Name = createDto.Name,
                    Description = createDto.Description
                };

                var createdRole = await _roleRepository.AddAsync(role);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, 
                    ApiResponse<RoleDto>.SuccessResult(MapToDto(createdRole), "Role created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<RoleDto>.ErrorResult(ex.Message));
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(Guid id, UpdateRoleDto updateDto)
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

                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(ApiResponse<RoleDto>.ErrorResult($"Role with ID {id} not found"));
                }

                // Update properties
                if (updateDto.Level.HasValue)
                    role.Level = updateDto.Level.Value;
                if (updateDto.Name != null)
                    role.Name = updateDto.Name;
                if (updateDto.Description != null)
                    role.Description = updateDto.Description;

                var updatedRole = await _roleRepository.UpdateAsync(role);
                return Ok(ApiResponse<RoleDto>.SuccessResult(MapToDto(updatedRole), "Role updated successfully"));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<RoleDto>.ErrorResult(ex.Message));
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
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Role with ID {id} not found"));
                }

                await _roleRepository.DeleteAsync(role);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult("An error occurred while deleting the role", new List<string> { ex.Message }));
            }
        }

        private static RoleDto MapToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Level = role.Level,
                Name = role.Name,
                Description = role.Description
            };
        }
    }
} 