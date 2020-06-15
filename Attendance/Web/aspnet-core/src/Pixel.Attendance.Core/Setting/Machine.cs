using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Setting
{
	[Table("Machines")]
    public class Machine : FullAuditedEntity 
    {

		public virtual string NameAr { get; set; }
		
		public virtual string NameEn { get; set; }
		
		public virtual string IpAddress { get; set; }
		
		public virtual string SubNet { get; set; }
		
		public virtual bool Status { get; set; }
		
		public virtual string Action { get; set; }
		
		public virtual string CmdStatus { get; set; }
		
		public virtual string Port { get; set; }
		
		public virtual bool Online { get; set; }
		
		public virtual DateTime LastImport { get; set; }
		
		public virtual int LastCount { get; set; }
		

		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }
		
    }
}