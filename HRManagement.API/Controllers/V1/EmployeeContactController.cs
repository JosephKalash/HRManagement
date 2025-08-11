using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Helpers;
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
    public class EmployeeContactController(IEmployeeContactService employeeContactService, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeContactService _employeeContactService = employeeContactService;
        private readonly IMapper _mapper = mapper;

        // GET: api/EmployeeContact
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeContactDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeContactDto>>>> GetAllEmployeeContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeContactService.GetPagedAsync(pageNumber, pageSize);
                var dtos = _mapper.Map<List<EmployeeContactDto>>(pagedResult.Items);
                return Ok(ApiResponse<PagedResult<EmployeeContactDto>>.SuccessResult(new PagedResult<EmployeeContactDto>
                {
                    Items = dtos,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalCount = pagedResult.TotalCount
                }, "Employee contacts retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<EmployeeContactDto>>.ErrorResult($"Internal server error: {ex.Message}"));
            }
        }

        // GET: api/EmployeeContact/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactById(Guid id)
        {
            var employeeContact = await _employeeContactService.GetByEmployeeIdAsync(id);
            return Ok(employeeContact);
        }

        // GET: api/EmployeeContact/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactByEmployeeId(Guid employeeId)
        {
            var employeeContact = await _employeeContactService.GetByEmployeeIdAsync(employeeId);
            return Ok(employeeContact);
        }

        // POST: api/EmployeeContact
        [HttpPost]
        public async Task<ActionResult<EmployeeContactDto>> CreateEmployeeContact(CreateEmployeeContactDto createEmployeeContactDto)
        {
            var employeeContact = await _employeeContactService.CreateAsync(createEmployeeContactDto);
            return CreatedAtAction(nameof(GetEmployeeContactById), new { id = employeeContact.Id }, employeeContact);
        }

        // PUT: api/EmployeeContact/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> UpdateEmployeeContact(Guid id, UpdateEmployeeContactDto updateDto)
        {
            var contact = await _employeeContactService.UpdateAsync(id, updateDto);
            return Ok(contact);
        }

        // DELETE: api/EmployeeContact/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployeeContact(Guid id)
        {
            await _employeeContactService.DeleteAsync(id);
            return NoContent();
        }
    }
}