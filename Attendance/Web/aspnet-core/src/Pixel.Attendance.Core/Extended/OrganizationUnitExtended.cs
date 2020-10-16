using Abp.Organizations;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.Attendance.Extended
{
    [Table("AbpOrganizationUnits")]
    public class OrganizationUnitExtended :OrganizationUnit
    {
        public OrganizationUnitExtended(int? tenantId, string displayName, long? parentId = null):base(tenantId, displayName, parentId)
        {
        }
        public long? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public User Manager { get; set; }
        public bool HasApprove { get; set; }
        public ICollection<OrganizationLocation> Locations { get; set; }
    }
}
