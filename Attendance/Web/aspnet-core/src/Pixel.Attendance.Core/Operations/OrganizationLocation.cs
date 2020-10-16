using Abp.Organizations;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using Pixel.Attendance.Extended;

namespace Pixel.Attendance.Operations
{
	[Table("OrganizationLocations")]
    [Audited]
    public class OrganizationLocation : FullAuditedEntity 
    {


		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnitExtended OrganizationUnitFk { get; set; }
		
		public virtual int? LocationId { get; set; }
		
        [ForeignKey("LocationId")]
		public Location LocationFk { get; set; }
		
    }
}