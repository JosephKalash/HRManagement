namespace HRManagement.Core.Entities;
    
public class EmployeeIdentityInfo
{
    public int? EmployeeCardNumber { get; set; }    // ecardno
    public string? IdentityCardNumber { get; set; } // identity_card

    // Passport
    public string? PassportNumber { get; set; }     // passport_no
    public string? PassportIssuePlace { get; set; } // passport_place
    public DateTime? PassportIssueDate { get; set; } // passport_date
    public DateTime? PassportExpiryDate { get; set; } // passport_expired_date

    // Residency
    public string? ResidencyNumber { get; set; }    // residentno
    public int? ResidencySponsor { get; set; }      // resident_sponsor
    public int? ResidencyPlaceId { get; set; }      // resident_placeid
    public DateTime? ResidencyIssueDate { get; set; } // resident_date
    public DateTime? ResidencyExpiryDate { get; set; } // resident_expired_date
}
