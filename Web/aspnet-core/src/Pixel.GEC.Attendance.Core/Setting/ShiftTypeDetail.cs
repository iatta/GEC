using Pixel.GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Setting
{
	[Table("ShiftTypeDetails")]
    public class ShiftTypeDetail : FullAuditedEntity 
    {

		public virtual bool InTimeFirstScan { get; set; }
		
		public virtual bool InTimeLastScan { get; set; }
		
		public virtual bool OutTimeFirstScan { get; set; }
		
		public virtual bool OutTimeLastScan { get; set; }
		
		public virtual int NoDuty { get; set; }
		

		public virtual int ShiftTypeId { get; set; }
		
        [ForeignKey("ShiftTypeId")]
		public ShiftType ShiftTypeFk { get; set; }
		
    }
}