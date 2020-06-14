using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace GEC.Attendance.Setting
{
	[Table("SystemConfigurations")]
    public class SystemConfiguration : FullAuditedEntity 
    {

		public virtual int TotalPermissionNumberPerMonth { get; set; }
		
		public virtual int TotalPermissionNumberPerWeek { get; set; }
		
		public virtual int TotalPermissionNumberPerDay { get; set; }
		
		public virtual int TotalPermissionHoursPerMonth { get; set; }
		
		public virtual int TotalPermissionHoursPerWeek { get; set; }
		
		public virtual int TotalPermissionHoursPerDay { get; set; }
		

    }
}