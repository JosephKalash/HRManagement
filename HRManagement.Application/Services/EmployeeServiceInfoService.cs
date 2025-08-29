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

        public async Task<EmployeeServiceInfoDto?> GetById(long id)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetById(id);
            return serviceInfo != null ? _mapper.Map<EmployeeServiceInfoDto>(serviceInfo) : null;
        }

        public async Task<IEnumerable<EmployeeServiceInfoDto>> GetAll()
        {
            var serviceInfos = await _employeeServiceInfoRepository.GetAll();
            return _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
        }

        public async Task<IEnumerable<EmployeeServiceInfoDto>> GetByEmployeeId(long employeeId)
        {
            var serviceInfos = await _employeeServiceInfoRepository.GetByEmployeeId(employeeId);
            return _mapper.Map<IEnumerable<EmployeeServiceInfoDto>>(serviceInfos);
        }

        public async Task<EmployeeServiceInfoDto?> GetActiveByEmployeeId(long employeeId)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetActiveByEmployeeId(employeeId);
            return serviceInfo != null ? _mapper.Map<EmployeeServiceInfoDto>(serviceInfo) : null;
        }

        public async Task<EmployeeServiceInfoDto> Create(CreateEmployeeServiceInfoDto createDto)
        {
            var serviceInfo = _mapper.Map<EmployeeServiceInfo>(createDto);
            var createdServiceInfo = await _employeeServiceInfoRepository.Add(serviceInfo);
            return _mapper.Map<EmployeeServiceInfoDto>(createdServiceInfo);
        }

        public async Task<EmployeeServiceInfoDto> Update(long id, UpdateEmployeeServiceInfoDto updateDto)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetById(id);
            if (serviceInfo == null)
                throw new ArgumentException("Employee service info not found");

            _mapper.Map(updateDto, serviceInfo);
            var updatedServiceInfo = await _employeeServiceInfoRepository.Update(serviceInfo);
            return _mapper.Map<EmployeeServiceInfoDto>(updatedServiceInfo);
        }

        public async Task Delete(long id)
        {
            var serviceInfo = await _employeeServiceInfoRepository.GetById(id);
            if (serviceInfo == null)
                throw new ArgumentException("Employee service info not found");

            await _employeeServiceInfoRepository.Delete(serviceInfo);
        }

        public async Task<bool> ActiveExists(long id)
        {
            return await _employeeServiceInfoRepository.ActiveExists(id);
        }

        public async Task<PagedResult<EmployeeServiceInfoDto>> GetPaged(int pageNumber, int pageSize)
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

        public async Task<PagedResult<EmployeeServiceInfoDto>> GetPagedByEmployeeId(long employeeId, int pageNumber, int pageSize)
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