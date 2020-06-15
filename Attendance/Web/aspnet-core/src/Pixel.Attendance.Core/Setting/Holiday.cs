using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Setting
{
	[Table("Holidays")]
    [Audited]
    public class Holiday : Entity 
    {

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }
		
		public virtual DateTime StartDate { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		

    }
}