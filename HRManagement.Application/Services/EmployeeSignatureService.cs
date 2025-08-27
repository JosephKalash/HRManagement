using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeSignatureService : IEmployeeSignatureService
    {
        private readonly IEmployeeSignatureRepository _signatureRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public EmployeeSignatureService(
            IEmployeeSignatureRepository signatureRepository,
            IEmployeeRepository employeeRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _signatureRepository = signatureRepository;
            _employeeRepository = employeeRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<EmployeeSignatureDto> Create(CreateEmployeeSignatureDto createDto, Stream? imageStream, string? fileName)
        {
            // Check if employee exists
            var employee = await _employeeRepository.GetById(createDto.EmployeeId);
            if (employee == null)
                throw new ArgumentException($"Employee with ID {createDto.EmployeeId} not found");

            // Check if signature already exists for this employee
            var existingSignature = await _signatureRepository.GetByEmployeeId(createDto.EmployeeId);
            if (existingSignature != null)
                throw new ArgumentException($"Signature already exists for employee with ID {createDto.EmployeeId}");

            if (imageStream == null || fileName == null)
                throw new ArgumentException("Image file is required");

            // Save the image
            var (filePath, savedFileName) = await _imageService.SaveImage(imageStream, "signatures", fileName);

            // Map DTO to entity using AutoMapper
            var signature = _mapper.Map<EmployeeSignature>(createDto);
            signature.FilePath = filePath;
            signature.OriginalFileName = fileName;

            var createdSignature = await _signatureRepository.AddAsync(signature);
            return _mapper.Map<EmployeeSignatureDto>(createdSignature);
        }

        public async Task<EmployeeSignatureDto?> GetById(long id)
        {
            var signature = await _signatureRepository.GetById(id);
            return _mapper.Map<EmployeeSignatureDto>(signature);
        }

        public async Task<EmployeeSignatureDto?> GetByEmployeeId(long employeeId)
        {
            var signature = await _signatureRepository.GetByEmployeeId(employeeId);
            return _mapper.Map<EmployeeSignatureDto>(signature);
        }

        public async Task<PagedResult<EmployeeSignatureDto>> GetPaged(int pageNumber, int pageSize)
        {
            var query = _signatureRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);

            var dtos = _mapper.Map<List<EmployeeSignatureDto>>(paged.Items);
            return new PagedResult<EmployeeSignatureDto>
            {
                Items = dtos,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<EmployeeSignatureDto> Update(long id, UpdateEmployeeSignatureDto updateDto)
        {
            var signature = await _signatureRepository.GetById(id);
            if (signature == null)
                throw new ArgumentException($"Signature with ID {id} not found");

            // Map update DTO to existing entity using AutoMapper
            _mapper.Map(updateDto, signature);
            signature.UpdatedAt = DateTime.UtcNow;

            var updatedSignature = await _signatureRepository.Update(signature);
            return _mapper.Map<EmployeeSignatureDto>(updatedSignature);
        }

        public async Task<EmployeeSignatureDto> UpdateSignatureImageAsync(long id, Stream imageStream, string fileName)
        {
            var signature = await _signatureRepository.GetById(id);
            if (signature == null)
                throw new ArgumentException($"Signature with ID {id} not found");

            // Delete old image if exists
            if (!string.IsNullOrEmpty(signature.FilePath))
                _imageService.DeleteImage(signature.FilePath);

            // Save new image
            var (filePath, savedFileName) = await _imageService.SaveImage(imageStream, "signatures", fileName);

            signature.FilePath = filePath;
            signature.OriginalFileName = fileName;
            signature.UpdatedAt = DateTime.UtcNow;

            var updatedSignature = await _signatureRepository.Update(signature);
            return _mapper.Map<EmployeeSignatureDto>(updatedSignature);
        }

        public async Task Delete(long id)
        {
            var signature = await _signatureRepository.GetById(id);
            if (signature == null)
                throw new ArgumentException($"Signature with ID {id} not found");

            // Delete the image file
            if (!string.IsNullOrEmpty(signature.FilePath))
                _imageService.DeleteImage(signature.FilePath);

            await _signatureRepository.Delete(signature);
        }
    }
}
