using Abp.Organizations;
using Pixel.GEC.Attendance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.GEC.Attendance.Extended
{
    public class OrganizationUnitExtended:OrganizationUnit
    {
        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public User Manager { get; set; }

    }
}
