using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Setting
{
	[Table("RamadanDates")]
    [Audited]
    public class RamadanDate : FullAuditedEntity 
    {

		[Required]
		public virtual string Year { get; set; }
		
		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		

    }
}