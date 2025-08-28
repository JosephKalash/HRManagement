using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class RankService(IRankRepository rankRepository, IMapper mapper) : IRankService
    {
        private readonly IRankRepository _rankRepository = rankRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<RankDto?> GetById(long id)
        {
            var rank = await _rankRepository.GetById(id);
            return rank != null ? _mapper.Map<RankDto>(rank) : null;
        }

        public async Task<RankDto?> GetByGuid(Guid guid)
        {
            var rank = await _rankRepository.GetByGuid(guid);
            return rank != null ? _mapper.Map<RankDto>(rank) : null;
        }

        public async Task<RankDto?> GetByOrder(int order)
        {
            var rank = await _rankRepository.GetByOrder(order);
            return rank != null ? _mapper.Map<RankDto>(rank) : null;
        }

        public async Task<PagedResult<RankDto>> GetPaged(int pageNumber, int pageSize)
        {
            var query = _rankRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtos = _mapper.Map<List<RankDto>>(paged.Items);
            return new PagedResult<RankDto>
            {
                Items = dtos,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<RankDto> Create(CreateRankDto dto)
        {
            // Ensure unique order
            var existing = await _rankRepository.GetByOrder(dto.Order);
            if (existing != null)
                throw new ArgumentException($"Rank with order {dto.Order} already exists");

            var entity = _mapper.Map<Rank>(dto);
            var created = await _rankRepository.AddAsync(entity);
            return _mapper.Map<RankDto>(created);
        }

        public async Task<RankDto> Update(long id, UpdateRankDto dto)
        {
            var rank = await _rankRepository.GetById(id);
            if (rank == null)
                throw new ArgumentException($"Rank with ID {id} not found");

            if (dto.Order.HasValue && dto.Order.Value != rank.Order)
            {
                var existing = await _rankRepository.GetByOrder(dto.Order.Value);
                if (existing != null && existing.Id != rank.Id)
                    throw new ArgumentException($"Another rank with order {dto.Order.Value} already exists");
            }

            _mapper.Map(dto, rank);
            rank.UpdatedAt = DateTime.UtcNow;
            var updated = await _rankRepository.Update(rank);
            return _mapper.Map<RankDto>(updated);
        }

        public async Task Delete(long id)
        {
            var rank = await _rankRepository.GetById(id);
            if (rank == null)
                throw new ArgumentException($"Rank with ID {id} not found");
            await _rankRepository.Delete(rank);
        }

        public async Task<bool> Exists(long id)
        {
            return await _rankRepository.ActiveExists(id);
        }
    }
}



