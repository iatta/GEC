using Pixel.Attendance.Operations;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("ProjectLocations")]
    [Audited]
    public class ProjectLocation : FullAuditedEntity 
    {


		public virtual int? ProjectId { get; set; }
		
        [ForeignKey("ProjectId")]
		public Project ProjectFk { get; set; }
		
		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
    }
}