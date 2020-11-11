using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Setting
{
	[Table("OverrideShifts")]
    [Audited]
    public class OverrideShift : FullAuditedEntity 
    {

		public virtual DateTime Day { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? ShiftId { get; set; }
		
        [ForeignKey("ShiftId")]
		public Shift ShiftFk { get; set; }
		
    }
}