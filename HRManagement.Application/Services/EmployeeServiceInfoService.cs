using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeServiceInfoService(IEmployeeServiceInfoRepository employeeServiceInfoRepository, IMapper mapper) : IEmployeeServiceInfoService
    {
        private readonly IEmployeeServiceInfoRepository _employeeServiceInfoRepository = employeeServiceInfoRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeServiceInfoDto?> GetByIdAsync(Guid id)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
            return serviceInfo != null ? _mapper.Map<EmployeeServiceInfoDto>(serviceInfo) : null;
        }

        public async Task<IEnumerable<EmployeeServiceInfoDto>> GetAllAsync()
        {
            var serviceInfos = await _employeeServiceInfoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
        }

        public async Task<IEnumerable<EmployeeServiceInfoDto>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var serviceInfos = await _employeeServiceInfoRepository.GetByEmployeeIdAsync(employeeId);
            return _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
        }

        public async Task<EmployeeServiceInfoDto?> GetActiveByEmployeeIdAsync(Guid employeeId)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetActiveByEmployeeIdAsync(employeeId);
            return serviceInfo != null ? _mapper.Map<EmployeeServiceInfoDto>(serviceInfo) : null;
        }

        public async Task<EmployeeServiceInfoDto> CreateAsync(CreateEmployeeServiceInfoDto createDto)
        {
            var serviceInfo = _mapper.Map<EmployeeServiceInfo>(createDto);
            var createdServiceInfo = await _employeeServiceInfoRepository.AddAsync(serviceInfo);
            return _mapper.Map<EmployeeServiceInfoDto>(createdServiceInfo);
        }

        public async Task<EmployeeServiceInfoDto> UpdateAsync(Guid id, UpdateEmployeeServiceInfoDto updateDto)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
            if (serviceInfo == null)
                throw new ArgumentException("Employee service info not found");

            _mapper.Map(updateDto, serviceInfo);
            var updatedServiceInfo = await _employeeServiceInfoRepository.UpdateAsync(serviceInfo);
            return _mapper.Map<EmployeeServiceInfoDto>(updatedServiceInfo);
        }

        public async Task DeleteAsync(Guid id)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetByIdAsync(id);
            if (serviceInfo == null)
                throw new ArgumentException("Employee service info not found");

            await _employeeServiceInfoRepository.DeleteAsync(serviceInfo);
        }

        public async Task<bool> ActiveExistsAsync(Guid id)
        {
            return await _employeeServiceInfoRepository.ActiveExistsAsync(id);
        }

        public async Task<PagedResult<EmployeeServiceInfoDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeServiceInfoRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeServiceInfoDto>>(paged.Items);
            return new PagedResult<EmployeeServiceInfoDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<PagedResult<EmployeeServiceInfoDto>> GetPagedByEmployeeIdAsync(Guid employeeId, int pageNumber, int pageSize)
        {
            var query = _employeeServiceInfoRepository.AsQueryable()
                .Where(esi => esi.EmployeeId == employeeId);
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeServiceInfoDto>>(paged.Items);
            return new PagedResult<EmployeeServiceInfoDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }
    }
}