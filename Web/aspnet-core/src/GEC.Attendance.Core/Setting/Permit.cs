using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
{
	[Table("Permits")]
    public class Permit : FullAuditedEntity 
    {

		[Required]
		public virtual string DescriptionAr { get; set; }
		
		[Required]
		public virtual string DescriptionEn { get; set; }


		public virtual bool LateIn { get; set; }
		public virtual bool EarlyOut { get; set; }

		public virtual bool OffShift { get; set; }
		public virtual bool FullDay { get; set; }

		public virtual bool InOut { get; set; }
		public virtual int? ToleranceIn { get; set; }
		
		public virtual int? ToleranceOut { get; set; }
		
		public virtual int? MaxNumberPerDay { get; set; }
		
		public virtual int? MaxNumberPerWeek { get; set; }
		
		public virtual int? MaxNumberPerMonth { get; set; }
		
		public virtual int? TotalHoursPerDay { get; set; }
		
		public virtual int? TotalHoursPerWeek { get; set; }
		
		public virtual int? TotalHoursPerMonth { get; set; }

		public virtual bool DeductType { get; set; }

		public virtual string Code { get; set; }
		public bool IsDeducted { get; set; }

		public ICollection<PermitType> PermitTypes { get; set; }

	}
}