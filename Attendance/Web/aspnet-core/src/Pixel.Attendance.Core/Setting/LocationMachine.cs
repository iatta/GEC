using Pixel.Attendance.Setting;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Setting
{
	[Table("LocationMachines")]
    [Audited]
    public class LocationMachine : FullAuditedEntity 
    {


		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
		public virtual int? MachineId { get; set; }
		
        [ForeignKey("MachineId")]
		public Machine MachineFk { get; set; }
		
    }
}