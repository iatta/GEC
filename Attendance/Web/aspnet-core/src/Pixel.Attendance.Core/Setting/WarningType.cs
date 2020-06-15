using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Setting
{
	[Table("WarningTypes")]
    public class WarningType : FullAuditedEntity 
    {

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }
		

    }
}