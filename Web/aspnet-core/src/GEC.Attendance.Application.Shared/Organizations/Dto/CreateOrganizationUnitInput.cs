using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace GEC.Attendance.Organizations.Dto
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }
        public long? ManagerId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; } 
    }
}