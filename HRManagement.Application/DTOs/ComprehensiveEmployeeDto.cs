using HRManagement.Application.DTOs;

namespace HRManagement.Application.DTOs
{
    public class ComprehensiveEmployeeDto : EmployeeDto
    {
        public EmployeeProfileDto? Profile { get; set; }
        public EmployeeContactDto? Contact { get; set; }
        public EmployeeServiceInfoDto? ActiveServiceInfo { get; set; }
        public List<EmployeeAssignmentDto>? Assignments { get; set; }
    }
}