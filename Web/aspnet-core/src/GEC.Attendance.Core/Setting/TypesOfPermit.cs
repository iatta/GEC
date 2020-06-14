using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
{
	[Table("TypesOfPermits")]
    public class TypesOfPermit : FullAuditedEntity 
    {

		[Required]
		public virtual string NameAr { get; set; }
		
		[Required]
		public virtual string NameEn { get; set; }

		public ICollection<PermitType> PermitTypes { get; set; }


	}
}