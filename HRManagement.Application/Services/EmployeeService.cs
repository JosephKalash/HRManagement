using System.Linq.Expressions;
using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Core.Entities;
using HRManagement.Core.Extensions;
using HRManagement.Core.Interfaces;
using HRManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Application.Services
{
    public class EmployeeService(
        IEmployeeRepository employeeRepository,
        IImageService imageService,
        IEmployeeProfileRepository employeeProfileRepository,
        IEmployeeServiceInfoRepository employeeServiceInfoRepository,
        IEmployeeAssignmentRepository employeeAssignmentRepository,
        IMapper mapper) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmployeeServiceInfoRepository _employeeServiceInfoRepository = employeeServiceInfoRepository;
        private readonly IEmployeeAssignmentRepository _employeeAssignmentRepository = employeeAssignmentRepository;

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await _employeeRepository.GetActiveEmployeesAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await _employeeRepository.SearchEmployeesAsync(searchTerm);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _employeeRepository.AsQueryable();
            var paged = await query.ToPagedResultAsync(pageNumber, pageSize);

            // Map entities to DTOs using AutoMapper
            var dtoList = _mapper.Map<List<EmployeeDto>>(paged.Items);
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
            var employee = _mapper.Map<Employee>(createDto);
            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto updateDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            _mapper.Map(updateDto, employee);
            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
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
            return await _employeeRepository.ActiveExistsAsync(id);
        }

        public async Task<string> UploadProfileImageAsync(Guid employeeId, Stream imageStream, string fileName)
        {

            var employeeProfile = await _employeeProfileRepository.GetByEmployeeIdAsync(employeeId) ?? throw new ArgumentException("Employee not found");

            // Save new image first
            var (filePath, _) = await _imageService.SaveImageAsync(imageStream, "employee-profiles", fileName);
            if (filePath == null)
                throw new ArgumentException("Image not valid");

            // Delete old image if exists (after successfully saving the new one)
            if (!string.IsNullOrEmpty(employeeProfile.ImagePath))
            {
                _imageService.DeleteImage(employeeProfile.ImagePath);
            }

            // Update the database with the new file path
            await _employeeProfileRepository.UpdateEmployeeImageAsync(employeeId, filePath);
            return filePath;
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

        public async Task<ComprehensiveEmployeeDto?> GetComprehensiveEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeWithAllDetailsAsync(id);
            if (employee == null)
            {
                return null;
            }
            return _mapper.Map<ComprehensiveEmployeeDto>(employee);
        }

        public async Task<List<EmployeeJobSummaryDto>> GetEmployeeJobSummaryAsync(Guid employeeId)
        {
            var jobSummaries = new List<EmployeeJobSummaryDto>();

            var activeServiceInfo = await _employeeServiceInfoRepository.GetActiveByEmployeeIdAsync(employeeId);
            if (activeServiceInfo != null)
            {
                jobSummaries.Add(_mapper.Map<EmployeeJobSummaryDto>(activeServiceInfo));
            }

            var assignments = await _employeeAssignmentRepository.GetActiveByEmployeeIdAsync(employeeId);
            if (assignments != null) // Check if the active assignment is found.
            {
                jobSummaries.Add(_mapper.Map<EmployeeJobSummaryDto>(assignments));
            }

            return jobSummaries;
        }

        public async Task<CurrentEmployeeSummaryDto?> GetCurrentEmployeeSummaryAsync(Guid userId)
        {
            // Match current user id to an employee record
            var employee = await _employeeRepository.GetByIdAsync(userId);
            if (employee == null)
            {
                return null;
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            // Get profile image path (optimized)
            var imagePath = await _employeeProfileRepository.GetEmployeeImagePathAsync(employee.Id);

            // Get job summary
            var jobSummary = await GetEmployeeJobSummaryAsync(employee.Id);

            return new CurrentEmployeeSummaryDto
            {
                Employee = employeeDto,
                ProfileImagePath = imagePath,
                JobSummary = jobSummary
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeeByRoleIdAsync(Guid roleId)
        {
            var employees = new HashSet<Employee>();

            // Get employees from service info
            var serviceInfoEmployees = await _employeeServiceInfoRepository.GetByRoleIdAsync(roleId);
            foreach (var serviceInfo in serviceInfoEmployees)
            {
                if (serviceInfo.Employee != null)
                {
                    employees.Add(serviceInfo.Employee);
                }
            }

            // Get employees from assignments
            var assignmentEmployees = await _employeeAssignmentRepository.GetByRoleIdAsync(roleId);
            foreach (var assignment in assignmentEmployees)
            {
                if (assignment.Employee != null)
                {
                    employees.Add(assignment.Employee);
                }
            }

            // Map to DTOs and return
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<ShortEmployeeDto?> GetByIdShort(Guid id)
        {
            ShortEmployeeDto shortEmployee = (await GetShortEmployeeBy(e => e.Id == id)).First();
            return shortEmployee;
        }

        private async Task<List<ShortEmployeeDto>> GetShortEmployeeBy(Expression<Func<Employee, bool>> predicate, bool isMultiple = false)
        {
            var query = _employeeRepository.AsQueryable();
            var shortEmployeeQuery = query
                .Where(predicate)
                .Select(e => new ShortEmployeeDto
                {
                    Id = e.Id,
                    MilitaryNumber = e.MilitaryNumber,
                    ArabicName = e.ArabicFirstName + " " + e.ArabicLastName,
                });
            if (isMultiple)
            {
                return await shortEmployeeQuery.ToListAsync();
            }
            var shortEmployee = await shortEmployeeQuery.FirstOrDefaultAsync() ?? throw new ArgumentException("Employee not found");
            return [shortEmployee];
        }

        public async Task<List<ShortEmployeeDto>?> GetByIdsShortAsync(List<Guid> ids)
        {
            List<ShortEmployeeDto> shortEmployees = await GetShortEmployeeBy(e => ids.Contains(e.Id));
            return shortEmployees;
        }

        public async Task<ShortEmployeeDto?> GetByMilitaryShort(int militaryNumber)
        {
            ShortEmployeeDto shortEmployee = (await GetShortEmployeeBy(e => e.MilitaryNumber == militaryNumber)).First();
            return shortEmployee;
        }

        public async Task<List<ShortEmployeeDto>?> GetByMilitariesShortList(List<int> militaryNumbers)
        {
            List<ShortEmployeeDto> shortEmployees = await GetShortEmployeeBy(e => militaryNumbers.Contains(e.MilitaryNumber));
            return shortEmployees;
        }
    }
}