using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
{
	[Table("ShiftTypes")]
    public class ShiftType : FullAuditedEntity 
    {

		public virtual string DescriptionEn { get; set; }
		
		public virtual string DescriptionAr { get; set; }
		
		public virtual int NumberOfDuties { get; set; }
		
		public virtual bool InScan { get; set; }
		
		public virtual bool OutScan { get; set; }
		
		public virtual bool CrossDay { get; set; }
		
		public virtual bool AlwaysAttend { get; set; }
		
		public virtual bool Open { get; set; }
		
		public virtual int MaxBoundryTime { get; set; }
		public virtual ICollection<ShiftTypeDetail> ShiftTypeDetails { get; set; }


	}
}