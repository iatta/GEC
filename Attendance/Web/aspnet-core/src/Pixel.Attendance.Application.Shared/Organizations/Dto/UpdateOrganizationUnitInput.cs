using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Organizations;
using Pixel.Attendance.Operations.Dtos;

namespace Pixel.Attendance.Organizations.Dto
{
    public class UpdateOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
        public long? ManagerId { get; set; }
        public bool HasApprove { get; set; }
        public ICollection<OrganizationLocationDto> Locations { get; set; }
    }
}