using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
{
	[Table("Locations")]
    public class Location : FullAuditedEntity 
    {

		public Location()
		{
			LocationCredentials = new HashSet<LocationCredential>();
		}

		public virtual string TitleAr { get; set; }
		
		public virtual string TitleEn { get; set; }

		public ICollection<LocationCredential> LocationCredentials { get; set; }
	}
}