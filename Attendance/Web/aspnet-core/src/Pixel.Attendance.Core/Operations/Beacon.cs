using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("Beacons")]
    [Audited]
    public class Beacon : FullAuditedEntity 
    {

		public virtual string Name { get; set; }
		
		public virtual string Uid { get; set; }
		
		public virtual int Minor { get; set; }
		
		public virtual int Major { get; set; }
		

    }
}