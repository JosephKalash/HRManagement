using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;
using HRManagement.Core.Repositories;

namespace HRManagement.Application.Services
{
    public class EmployeeContactService(IEmployeeContactRepository employeeContactRepository, IMapper mapper) : IEmployeeContactService
    {
        private readonly IEmployeeContactRepository _employeeContactRepository = employeeContactRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeContactDto?> GetByIdAsync(Guid id)
        {
            var contact = await _employeeContactRepository.GetByIdAsync(id);
            return contact != null ? _mapper.Map<EmployeeContactDto>(contact) : null;
        }

        public async Task<EmployeeContactDto?> GetByEmployeeIdAsync(Guid employeeId)
        {
            var contact = await _employeeContactRepository.GetEmployeeContactByEmployeeId(employeeId);
            return contact != null ? _mapper.Map<EmployeeContactDto>(contact) : null;
        }

        public async Task<IEnumerable<EmployeeContactDto>> GetAllAsync()
        {
            var contacts = await _employeeContactRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeContactDto>>(contacts);
        }

        public async Task<EmployeeContactDto> CreateAsync(CreateEmployeeContactDto createDto)
        {
            var contact = _mapper.Map<EmployeeContact>(createDto);
            var createdContact = await _employeeContactRepository.AddAsync(contact);
            return _mapper.Map<EmployeeContactDto>(createdContact);
        }

        public async Task<EmployeeContactDto> UpdateAsync(Guid id, UpdateEmployeeContactDto updateDto)
        {
            var contact = await _employeeContactRepository.GetByIdAsync(id);
            if (contact == null)
                throw new ArgumentException("Employee contact not found");

            _mapper.Map(updateDto, contact);
            var updatedContact = await _employeeContactRepository.UpdateAsync(contact);
            return _mapper.Map<EmployeeContactDto>(updatedContact);
        }

        public async Task DeleteAsync(Guid id)
        {
            var contact = await _employeeContactRepository.GetByIdAsync(id);
            if (contact == null)
                throw new ArgumentException("Employee contact not found");

            await _employeeContactRepository.DeleteAsync(contact);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _employeeContactRepository.ActiveExistsAsync(id);
        }

        public async Task<PagedResult<EmployeeContactDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeContactRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeContactDto>>(paged.Items);
            return new PagedResult<EmployeeContactDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }
    }
}