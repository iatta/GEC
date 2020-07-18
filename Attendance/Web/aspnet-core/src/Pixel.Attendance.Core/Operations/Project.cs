using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
	[Table("Projects")]
    [Audited]
    public class Project : FullAuditedEntity 
    {

		public virtual string NameAr { get; set; }
		
		public virtual string NameEn { get; set; }

		public virtual string Code { get; set; }
		public virtual string Number { get; set; }
		public virtual long? ManagerId { get; set; }
		
        [ForeignKey("ManagerId")]
		public User ManagerFk { get; set; }
		
		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }

		public ICollection<ProjectUser> Users { get; set; }

		public ICollection<ProjectMachine> Machines { get; set; }

	}
}