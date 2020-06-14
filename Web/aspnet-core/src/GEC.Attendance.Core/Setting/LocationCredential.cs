using GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace GEC.Attendance.Setting
{
	[Table("LocationCredentials")]
    public class LocationCredential : FullAuditedEntity 
    {

		public virtual double Longitude { get; set; }
		
		public virtual double Latitude { get; set; }
		

		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
    }
}