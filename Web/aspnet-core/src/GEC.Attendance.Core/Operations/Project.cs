using GEC.Attendance.Authorization.Users;
using Abp.Organizations;
using GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace GEC.Attendance.Operations
{
	[Table("Projects")]
    [Audited]
    public class Project : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }
		

		public virtual long? ManagerId { get; set; }
		
        [ForeignKey("ManagerId")]
		public User ManagerFk { get; set; }
		
		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }
		
		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
    }
}