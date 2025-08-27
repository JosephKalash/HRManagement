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


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<EmployeeContactDto>>), 200)]

        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeContactDto>>>> GetAllEmployeeContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _employeeContactService.GetPaged(pageNumber, pageSize);
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


        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactById(long id)
        {
            var employeeContact = await _employeeContactService.GetByEmployeeId(id);
            return Ok(employeeContact);
        }


        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactByEmployeeId(long employeeId)
        {
            var employeeContact = await _employeeContactService.GetByEmployeeId(employeeId);
            return Ok(employeeContact);
        }


        [HttpPost]
        public async Task<ActionResult<EmployeeContactDto>> CreateEmployeeContact(CreateEmployeeContactDto createEmployeeContactDto)
        {
            var employeeContact = await _employeeContactService.Create(createEmployeeContactDto);
            return CreatedAtAction(nameof(GetEmployeeContactById), new { id = employeeContact.Id }, employeeContact);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> UpdateEmployeeContact(long id, UpdateEmployeeContactDto updateDto)
        {
            var contact = await _employeeContactService.Update(id, updateDto);
            return Ok(contact);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployeeContact(long id)
        {
            await _employeeContactService.Delete(id);
            return NoContent();
        }
    }
}