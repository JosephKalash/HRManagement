using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        IRoleRepository roleRepo,
        IMapper mapper) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IEmployeeProfileRepository _employeeProfileRepository = employeeProfileRepository;
        private readonly IImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmployeeServiceInfoRepository _employeeServiceInfoRepository = employeeServiceInfoRepository;
        private readonly IEmployeeAssignmentRepository _employeeAssignmentRepository = employeeAssignmentRepository;
        private readonly IRoleRepository _roleRepo = roleRepo;

        public async Task<EmployeeDto?> GetById(long id)
        {
            // var employee = await _employeeRepository.GetById(id);
            var query = _employeeRepository.AsQueryable();
            var employee = await query
                .Where(e => e.Id == id)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return employee;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            var employees = await _employeeRepository.GetAll();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployees()
        {
            var employees = await _employeeRepository.GetActiveEmployees();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployees(string searchTerm)
        {
            var employees = await _employeeRepository.SearchEmployees(searchTerm);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<PagedResult<EmployeeDto>> GetPaged(int pageNumber, int pageSize)
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

        public async Task<EmployeeDto> Create(CreateEmployeeDto createDto)
        {
            var employee = _mapper.Map<Employee>(createDto);
            var createdEmployee = await _employeeRepository.Add(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task<EmployeeDto> Update(long id, UpdateEmployeeDto updateDto)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            _mapper.Map(updateDto, employee);
            var updatedEmployee = await _employeeRepository.Update(employee);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }

        public async Task Delete(long id)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            await _employeeRepository.Delete(employee);
        }

        public async Task<bool> Exists(long id)
        {
            return await _employeeRepository.ActiveExists(id);
        }

        public async Task<string> UploadProfileImage(long employeeId, Stream imageStream, string fileName)
        {

            var employeeProfile = await _employeeProfileRepository.GetByEmployeeId(employeeId) ?? throw new ArgumentException("Employee not found");

            // Save new image first
            var (filePath, _) = await _imageService.SaveImage(imageStream, "employee-profiles", fileName);
            if (filePath == null)
                throw new ArgumentException("Image not valid");

            // Delete old image if exists (after successfully saving the new one)
            if (!string.IsNullOrEmpty(employeeProfile.ImagePath))
            {
                _imageService.DeleteImage(employeeProfile.ImagePath);
            }

            // Update the database with the new file path
            await _employeeProfileRepository.UpdateEmployeeImage(employeeId, filePath);
            return filePath;
        }

        public async Task DeleteProfileImage(long employeeId)
        {
            var empProfile = await _employeeProfileRepository.GetByEmployeeId(employeeId);
            if (empProfile == null)
                throw new ArgumentException("Employee not found");

            if (!string.IsNullOrEmpty(empProfile.ImagePath))
            {
                _imageService.DeleteImage(empProfile.ImagePath);
                empProfile.ImagePath = null;
                await _employeeProfileRepository.Update(empProfile);
            }
        }

        public async Task<EmployeeProfile?> GetProfileByEmployeeId(long id)
        {
            var empProfile = await _employeeProfileRepository.GetByEmployeeId(id);
            if (empProfile == null)
                throw new ArgumentException("Employee not found");

            return empProfile;
        }

        public async Task<ComprehensiveEmployeeDto?> GetComprehensiveEmployeeById(long id)
        {
            var employee = await _employeeRepository.GetEmployeeWithAllDetails(id);
            if (employee == null)
            {
                return null;
            }
            return _mapper.Map<ComprehensiveEmployeeDto>(employee);
        }

        public async Task<List<EmployeeJobSummaryDto>> GetEmployeeJobSummary(long employeeId)
        {
            var jobSummaries = new List<EmployeeJobSummaryDto>();

            var activeServiceInfo = await _employeeServiceInfoRepository.GetActiveByEmployeeId(employeeId);
            if (activeServiceInfo != null)
            {
                jobSummaries.Add(_mapper.Map<EmployeeJobSummaryDto>(activeServiceInfo));
            }

            var assignments = await _employeeAssignmentRepository.GetActiveByEmployeeId(employeeId);
            if (assignments != null) // Check if the active assignment is found.
            {
                jobSummaries.Add(_mapper.Map<EmployeeJobSummaryDto>(assignments));
            }

            return jobSummaries;
        }

        public async Task<CurrentEmployeeSummaryDto?> GetCurrentEmployeeSummary(long userId)
        {
            var employee = await _employeeRepository.GetById(userId);
            if (employee == null)
            {
                return null;
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            var imagePath = await _employeeProfileRepository.GetEmployeeImagePath(employee.Id);

            var jobSummary = await GetEmployeeJobSummary(employee.Id);

            return new CurrentEmployeeSummaryDto
            {
                Employee = employeeDto,
                ProfileImagePath = imagePath,
                JobSummary = jobSummary
            };
        }

        public async Task<List<EmployeeDto>> GetEmployeeByRoleId(long roleId)
        {
            var employees = new HashSet<Employee>();

            var serviceInfoEmployees = await _employeeServiceInfoRepository.GetByRoleId(roleId);
            foreach (var serviceInfo in serviceInfoEmployees)
            {
                if (serviceInfo.Employee != null)
                {
                    employees.Add(serviceInfo.Employee);
                }
            }

            var assignmentEmployees = await _employeeAssignmentRepository.GetByRoleId(roleId);
            foreach (var assignment in assignmentEmployees)
            {
                if (assignment.Employee != null)
                {
                    employees.Add(assignment.Employee);
                }
            }

            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<ShortEmployeeDto> GetByIdShort(long id)
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
                    RankName = e.EmployeeRanks
                                    .Where(er => er.IsActive)
                                    .Select(er => er.Rank.Name).FirstOrDefault() ?? string.Empty
                });
            if (isMultiple)
            {
                return await shortEmployeeQuery.ToListAsync();
            }
            var shortEmployee = await shortEmployeeQuery.FirstOrDefaultAsync() ?? throw new ArgumentException("Employee not found");
            return [shortEmployee];
        }

        public async Task<List<ShortEmployeeDto>> GetByIdsShort(List<Guid> guids) //external
        {
            List<ShortEmployeeDto> shortEmployees = await GetShortEmployeeBy(e => guids.Contains(e.Guid));
            return shortEmployees;
        }

        public async Task<ShortEmployeeDto> GetByMilitaryShort(int militaryNumber)
        {
            ShortEmployeeDto shortEmployee = (await GetShortEmployeeBy(e => e.MilitaryNumber == militaryNumber)).First();
            return shortEmployee;
        }

        public async Task<List<ShortEmployeeDto>> GetByMilitariesShortList(List<int> militaryNumbers)
        {
            List<ShortEmployeeDto> shortEmployees = await GetShortEmployeeBy(e => militaryNumbers.Contains(e.MilitaryNumber));
            return shortEmployees;
        }

        public async Task<List<Guid>> GetEmployeeRoleGuids(Guid employeeGuid) //external 
        {
            var emp = await _employeeRepository.GetByGuid(employeeGuid) ?? throw new ArgumentException("Employee not found");
            var jobs = await GetEmployeeJobSummary(emp.Id);
            var roleIds = jobs.Select(j => j.JobRoleId).Distinct().ToList();
            var guids = await _roleRepo.GetGuidsByIds(roleIds);
            return guids;
        }
    }
}