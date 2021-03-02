using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Setting
{
	[Table("TaskTypes")]
    [Audited]
    public class TaskType : FullAuditedEntity 
    {

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }
		
		[Required]
		public virtual string Number { get; set; }
		

    }
}