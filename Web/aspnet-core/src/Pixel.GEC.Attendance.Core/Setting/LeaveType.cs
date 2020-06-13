using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Setting
{
	[Table("LeaveTypes")]
    public class LeaveType : Entity 
    {

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }
		
		[Required]
		public virtual string Code { get; set; }
		

    }
}