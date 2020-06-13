using Pixel.GEC.Attendance.Operations;
using Pixel.GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Operations
{
	[Table("TimeProfileDetails")]
    public class TimeProfileDetail : FullAuditedEntity 
    {

		public virtual int Day { get; set; }
		
		public virtual int ShiftNo { get; set; }
		

		public virtual int? TimeProfileId { get; set; }
		
        [ForeignKey("TimeProfileId")]
		public TimeProfile TimeProfileFk { get; set; }
		
		public virtual int? ShiftId { get; set; }
		
        [ForeignKey("ShiftId")]
		public Shift ShiftFk { get; set; }
		
    }
}