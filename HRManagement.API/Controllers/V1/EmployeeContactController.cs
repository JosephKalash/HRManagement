using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Helpers;
using HRManagement.Core.Entities;
using HRManagement.Core.Models;
using HRManagement.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeContactController(IEmployeeContactRepository employeeContactRepository, IMapper mapper) : ControllerBase
    {
        private readonly IEmployeeContactRepository _employeeContactRepository = employeeContactRepository;
        private readonly IMapper _mapper = mapper;

        // GET: api/EmployeeContact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeContactDto>>> GetAllEmployeeContacts()
        {
            var employeeContacts = await _employeeContactRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeContactDto>>(employeeContacts));
        }

        // GET: api/EmployeeContact/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactById(Guid id)
        {
            var employeeContact = await _employeeContactRepository.GetEmployeeContactByEmployeeId(id);
            return Ok(_mapper.Map<EmployeeContactDto>(employeeContact));
        }

        // GET: api/EmployeeContact/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<EmployeeContactDto>> GetEmployeeContactByEmployeeId(Guid employeeId)
        {
            var employeeContact = await _employeeContactRepository.GetEmployeeContactByEmployeeId(employeeId);
            return Ok(_mapper.Map<EmployeeContactDto>(employeeContact));
        }

        // POST: api/EmployeeContact
        [HttpPost]
        public async Task<ActionResult<EmployeeContactDto>> CreateEmployeeContact(CreateEmployeeContactDto createEmployeeContactDto)
        {
            var employeeContact = _mapper.Map<EmployeeContact>(createEmployeeContactDto);
            var newEmployeeContact = await _employeeContactRepository.AddAsync(employeeContact);
            return CreatedAtAction(nameof(GetEmployeeContactById), new { id = newEmployeeContact.Id }, _mapper.Map<EmployeeContactDto>(newEmployeeContact));
        }

        // PUT: api/EmployeeContact/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeContactDto>> UpdateEmployeeContact(Guid id, UpdateEmployeeContactDto updateDto)
        {
            var contact = await _employeeContactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound(ApiResponse<EmployeeContactDto>.ErrorResult($"Employee profile with ID {id} not found"));
            }

            UpdateContactProperties(updateDto, contact);
            var updatedEmployeeContact = await _employeeContactRepository.UpdateAsync(contact);
            return Ok(_mapper.Map<EmployeeContactDto>(updatedEmployeeContact));
        }

        private static void UpdateContactProperties(UpdateEmployeeContactDto updateDto, EmployeeContact contact)
        {
            updateDto.Email.SetIfNotNull(val => contact.Email = val);
            updateDto.MobileNumber.SetIfNotNull(val => contact.MobileNumber = val);
            updateDto.SecondMobileNumber.SetIfNotNull(val => contact.SecondMobileNumber = val);
            updateDto.Address.SetIfNotNull(val => contact.Address = val);
        }

        // DELETE: api/EmployeeContact/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployeeContact(Guid id)
        {
            var contact = await _employeeContactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound(ApiResponse<EmployeeContactDto>.ErrorResult($"Employee profile with ID {id} not found"));
            }
            await _employeeContactRepository.DeleteAsync(contact);
            return NoContent();
        }
    }
}