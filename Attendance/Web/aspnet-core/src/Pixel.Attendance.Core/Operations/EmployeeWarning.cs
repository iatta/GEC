using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Operations
{
	[Table("EmployeeWarnings")]
    public class EmployeeWarning : FullAuditedEntity 
    {

		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? WarningTypeId { get; set; }
		
        [ForeignKey("WarningTypeId")]
		public WarningType WarningTypeFk { get; set; }
		
    }
}