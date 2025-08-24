using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeProfileService(IEmployeeProfileRepository employeeProfileRepository, IImageService imageService, IMapper mapper) : IEmployeeProfileService
    {
        private readonly IImageService _imageService = imageService;
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeProfileDto?> GetById(Guid id)
        {
            var profile = await _employeeProfileRepository.GetById(id);
            return profile != null ? _mapper.Map<EmployeeProfileDto>(profile) : null;
        }

        public async Task<EmployeeProfileDto?> GetByEmployeeId(Guid employeeId)
        {
            var profile = await _employeeProfileRepository.GetByEmployeeId(employeeId);
            return profile != null ? _mapper.Map<EmployeeProfileDto>(profile) : null;
        }

        public async Task<IEnumerable<EmployeeProfileDto>> GetAll()
        {
            var profiles = await _employeeProfileRepository.GetAll();
            return _mapper.Map<IEnumerable<EmployeeProfileDto>>(profiles);
        }

        public async Task<EmployeeProfileDto> Create(CreateEmployeeProfileDto createDto, Stream? imageStream, string? imageName)
        {
            var profile = _mapper.Map<EmployeeProfile>(createDto);

            if (imageStream != null && imageStream.Length > 0)
            {
                var (savedRelativePath, _) = await _imageService.SaveImage(imageStream, "profiles", imageName!);
                if (!string.IsNullOrEmpty(savedRelativePath))
                {
                    profile.ImagePath = '/' + savedRelativePath;
                }
            }

            var createdProfile = await _employeeProfileRepository.AddAsync(profile);
            return _mapper.Map<EmployeeProfileDto>(createdProfile);
        }


        public async Task<EmployeeProfileDto> Update(Guid id, UpdateEmployeeProfileDto updateDto, Stream? stream, string? fileName = null)
        {
            var profile = await _employeeProfileRepository.GetById(id) ?? throw new ArgumentException("Employee profile not found");
            _mapper.Map(updateDto, profile);

            if (stream != null && stream.Length > 0)
            {
                var (savedRelativePath, _) = await _imageService.SaveImage(stream, "profiles", fileName!);
                if (!string.IsNullOrEmpty(savedRelativePath))
                {
                    profile.ImagePath = '/' + savedRelativePath;
                }
            }

            var updatedProfile = await _employeeProfileRepository.Update(profile);
            return _mapper.Map<EmployeeProfileDto>(updatedProfile);
        }

        public async Task Delete(Guid id)
        {
            var profile = await _employeeProfileRepository.GetById(id) ?? throw new ArgumentException("Employee profile not found");
            await _employeeProfileRepository.Delete(profile);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _employeeProfileRepository.ActiveExists(id);
        }

        public async Task<PagedResult<EmployeeProfileDto>> GetPaged(int pageNumber, int pageSize)
        {
            var query = _employeeProfileRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            var dtoList = _mapper.Map<List<EmployeeProfileDto>>(paged.Items);
            return new PagedResult<EmployeeProfileDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task UpdateEmployeeImage(Guid employeeId, string imagePath)
        {
            await _employeeProfileRepository.UpdateEmployeeImage(employeeId, imagePath);
        }
    }
}