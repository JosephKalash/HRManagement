using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;

namespace HRManagement.Application.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository, IImageService imageService, IEmployeeProfileRepository employeeProfileRepository) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IImageService _imageService = imageService;

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null ? MapToDto(employee) : null;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await _employeeRepository.GetActiveEmployeesAsync();
            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await _employeeRepository.SearchEmployeesAsync(searchTerm);
            return employees.Select(MapToDto);
        }

        public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);
            // Map entities to DTOs
            var dtoList = paged.Items.Select(MapToDto).ToList();
            return new PagedResult<EmployeeDto>
            {
                Items = dtoList,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
        {
            var employee = new Employee
            {
                MilitaryNumber = createDto.MilitaryNumber,
                ArabicFirstName = createDto.ArabicFirstName,
                ArabicMiddleName = createDto.ArabicMiddleName,
                ArabicLastName = createDto.ArabicLastName,
                EnglishFirstName = createDto.EnglishFirstName,
                EnglishMiddleName = createDto.EnglishMiddleName,
                EnglishLastName = createDto.EnglishLastName,
                Gender = createDto.Gender,
                DateOfBirth = createDto.DateOfBirth,
                IdNumber = createDto.IdNumber,
                IsActive = true
            };

            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return MapToDto(createdEmployee);
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto updateDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            if (updateDto.MilitaryNumber.HasValue)
                employee.MilitaryNumber = updateDto.MilitaryNumber.Value;
            if (updateDto.ArabicFirstName != null)
                employee.ArabicFirstName = updateDto.ArabicFirstName;
            if (updateDto.ArabicMiddleName != null)
                employee.ArabicMiddleName = updateDto.ArabicMiddleName;
            if (updateDto.ArabicLastName != null)
                employee.ArabicLastName = updateDto.ArabicLastName;
            if (updateDto.EnglishFirstName != null)
                employee.EnglishFirstName = updateDto.EnglishFirstName;
            if (updateDto.EnglishMiddleName != null)
                employee.EnglishMiddleName = updateDto.EnglishMiddleName;
            if (updateDto.EnglishLastName != null)
                employee.EnglishLastName = updateDto.EnglishLastName;
            if (updateDto.Gender.HasValue)
                employee.Gender = updateDto.Gender.Value;
            if (updateDto.DateOfBirth.HasValue)
                employee.DateOfBirth = updateDto.DateOfBirth.Value;
            if (updateDto.IdNumber != null)
                employee.IdNumber = updateDto.IdNumber;
            if (updateDto.IsActive.HasValue)
                employee.IsActive = updateDto.IsActive.Value;

            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
            return MapToDto(updatedEmployee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _employeeRepository.ExistsAsync(id);
        }

        public async Task<string> UploadProfileImageAsync(Guid employeeId, Stream imageFile, string originalFileName)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Invalid image file.");
            }

            var employeeProfile = await _employeeProfileRepository.GetByEmployeeIdAsync(employeeId) ?? throw new ArgumentException("Employee not found");

            // Save new image first
            var (filePath, _) = await _imageService.SaveImageAsync(imageFile, "employee-profiles", originalFileName);

            // Delete old image if exists (after successfully saving the new one)
            if (!string.IsNullOrEmpty(employeeProfile.ImagePath))
            {
                _imageService.DeleteImage(employeeProfile.ImagePath);
            }

            // Update the database with the new file path
            await _employeeProfileRepository.UpdateEmployeeImageAsync(employeeId, filePath);
            return filePath;
        }

        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                MilitaryNumber = employee.MilitaryNumber,
                ArabicFirstName = employee.ArabicFirstName,
                ArabicMiddleName = employee.ArabicMiddleName,
                ArabicLastName = employee.ArabicLastName,
                EnglishFirstName = employee.EnglishFirstName,
                EnglishMiddleName = employee.EnglishMiddleName,
                EnglishLastName = employee.EnglishLastName,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                IdNumber = employee.IdNumber,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }

        public async Task DeleteProfileImageAsync(Guid employeeId)
        {
            var empProfile = await _employeeProfileRepository.GetByEmployeeIdAsync(employeeId);
            if (empProfile == null)
                throw new ArgumentException("Employee not found");

            if (!string.IsNullOrEmpty(empProfile.ImagePath))
            {
                _imageService.DeleteImage(empProfile.ImagePath);
                empProfile.ImagePath = null;
                await _employeeProfileRepository.UpdateAsync(empProfile);
            }
        }

        public async Task<EmployeeProfile?> GetProfileByEmployeeIdAsync(Guid id)
        {
            var empProfile = await _employeeProfileRepository.GetByEmployeeIdAsync(id);
            if (empProfile == null)
                throw new ArgumentException("Employee not found");

            return empProfile;

        }


    }
}