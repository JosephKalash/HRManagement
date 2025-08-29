using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Interfaces;

namespace HRManagement.Application.Services
{
    public class EmployeeRankService(IEmployeeRankRepository employeeRankRepository, IMapper mapper)
        : IEmployeeRankService
    {
        private readonly IEmployeeRankRepository _employeeRankRepository = employeeRankRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeRankDto?> GetById(long id)
        {
            var rank = await _employeeRankRepository.GetById(id);
            return rank != null ? _mapper.Map<EmployeeRankDto>(rank) : null;
        }

        public async Task<List<EmployeeRankDto>> GetAll()
        {
            var ranks = await _employeeRankRepository.GetAll();
            return _mapper.Map<List<EmployeeRankDto>>(ranks);
        }

        public async Task<List<EmployeeRankDto>> GetByEmployeeId(long employeeId)
        {
            var ranks = await _employeeRankRepository.GetByEmployeeId(employeeId);
            return _mapper.Map<List<EmployeeRankDto>>(ranks);
        }

        public async Task<EmployeeRankDto> Create(CreateEmployeeRankDto createDto)
        {
            var entity = _mapper.Map<EmployeeRank>(createDto);
            var created = await _employeeRankRepository.Add(entity);
            return _mapper.Map<EmployeeRankDto>(created);
        }

        public async Task<EmployeeRankDto> Update(long id, UpdateEmployeeRankDto updateDto)
        {
            var entity = await _employeeRankRepository.GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"EmployeeRank with id {id} not found.");

            _mapper.Map(updateDto, entity); // update entity with dto values
            var updated = await _employeeRankRepository.Update(entity);
            return _mapper.Map<EmployeeRankDto>(updated);
        }

        public async Task<List<EmployeeRankDto>> GetByRankId(long rankId)
        {
            var ranks = await _employeeRankRepository.GetByRankId(rankId);
            return _mapper.Map<List<EmployeeRankDto>>(ranks);
        }

        public async Task Delete(long id)
        {
            var entity = await _employeeRankRepository.GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"EmployeeRank with id {id} not found.");

            await _employeeRankRepository.Delete(entity);
        }

        public async Task<bool> ActiveExists(long id)
        {
            return await _employeeRankRepository.ActiveExists(id);
        }
    }
}
