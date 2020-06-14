using Abp.Organizations;
using Pixel.GEC.Attendance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.GEC.Attendance.Extended
{
    [Table("AbpOrganizationUnits")]
    public class OrganizationUnitExtended:OrganizationUnit
    {
        public OrganizationUnitExtended(int? tenantId, string displayName, long? parentId = null, long? managerId = null) : base(tenantId,displayName,parentId)
        {
            this.ManagerId = managerId;
        }
        public long? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public User Manager { get; set; }

    }
}
