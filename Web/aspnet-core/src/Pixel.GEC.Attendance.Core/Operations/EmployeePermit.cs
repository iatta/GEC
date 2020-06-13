using Pixel.GEC.Attendance.Authorization.Users;
using Pixel.GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Operations
{
	[Table("EmployeePermits")]
    public class EmployeePermit : FullAuditedEntity 
    {

		public virtual int? FromTime { get; set; }
		
		public virtual int? ToTime { get; set; }
		
		public virtual DateTime PermitDate { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual bool Status { get; set; }
		
		public virtual string Note { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? PermitId { get; set; }
		
        [ForeignKey("PermitId")]
		public Permit PermitFk { get; set; }
		
    }
}