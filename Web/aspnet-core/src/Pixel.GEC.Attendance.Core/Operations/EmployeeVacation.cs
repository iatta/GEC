using Pixel.GEC.Attendance.Authorization.Users;
using Pixel.GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.GEC.Attendance.Operations
{
	[Table("EmployeeVacations")]
    [Audited]
    public class EmployeeVacation : FullAuditedEntity 
    {

		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual bool Status { get; set; }
		
		public virtual string Note { get; set; }
		

		public virtual long UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int LeaveTypeId { get; set; }
		
        [ForeignKey("LeaveTypeId")]
		public LeaveType LeaveTypeFk { get; set; }
		
    }
}