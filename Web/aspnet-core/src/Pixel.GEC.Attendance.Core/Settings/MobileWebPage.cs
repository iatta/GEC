using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Settings
{
	[Table("MobileWebPages")]
    public class MobileWebPage : FullAuditedEntity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Content { get; set; }
		

    }
}