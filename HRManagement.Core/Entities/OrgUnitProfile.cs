using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.Core.Entities
{
    public class OrgUnitProfile : BaseEntity
    {

        public long OrgUnitId { get; set; }
        
        public OrgUnit? OrgUnit { get; set; }

        // [Column(TypeName = "nvarchar(max)")]
        public string? Specialization { get; set; }

        public int AllowedForcesOff { get; set; } = 100; //center_forces
        
        [StringLength(20)]
        public string? UnitCodeExtra { get; set; }
        
        [StringLength(20)]
        public string? Phone { get; set; } //centerPhonse
        
        public int? Link { get; set; } //center_link
        
        [StringLength(200)]
        public string? JobDescPath { get; set; } //center_job_desc
        
        [StringLength(200)]
        public string? Stamp { get; set; }//center_stamp
        
        public bool ManagerOffLeaderApproval { get; set; } = false; // vacation_leader_approv
        
        [StringLength(20)]
        public string? CostNumber { get; set; } //cost_no
    }
}