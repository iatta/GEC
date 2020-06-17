using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("UserShifts")]
    [Audited]
    public class UserShift : FullAuditedEntity 
    {

		public virtual DateTime Date { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? ShiftId { get; set; }
		
        [ForeignKey("ShiftId")]
		public Shift ShiftFk { get; set; }
		
    }
}