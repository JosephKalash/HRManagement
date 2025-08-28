using System.ComponentModel.DataAnnotations;

namespace HRManagement.Core.Entities
{
    public class EmployeeContact : BaseEntity
    {
        public long EmployeeId { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        public required string MobileNumber { get; set; }
        [Phone]
        public string? SecondMobileNumber { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
        [Phone]
        public string? SecondPhoneNumber { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
// public class EmployeeContactInfo
// {
//     // Phones
//     public string? Phone { get; set; }              // ephone
//     public string? MobilePrimary { get; set; }      // emobile
//     public string? MobileSecondary { get; set; }    // other_phone
//     public string? Extension { get; set; }          // extention_phone_no
//     public string? FaxNumber { get; set; }          // fax_no

//     // Email
//     public string? Email { get; set; }              // email
//     public string? OfficialEmail { get; set; }      // moi_email

//     // Address
//     public string? Street { get; set; }             // street_no
//     public string? HouseNumber { get; set; }        // house_no
//     public string? FlatNumber { get; set; }         // flat_no
//     public string? Area { get; set; }               // areaid (linked lookup)
//     public string? City { get; set; }               // cityid (linked lookup)
//     public string? Pobox { get; set; }              // emp_pobox
//     public string? AddressDetails { get; set; }     // address (free-text)
// }
