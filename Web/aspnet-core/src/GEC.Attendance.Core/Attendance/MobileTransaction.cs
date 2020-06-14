using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace GEC.Attendance.Attendance
{
	[Table("MobileTransactions")]
    public class MobileTransaction : FullAuditedEntity 
    {

		public virtual string EmpCode { get; set; }
		
		public virtual string MachineID { get; set; }
		
		public virtual bool TransStatus { get; set; }
		
		public virtual DateTime TransDate { get; set; }
		
		public virtual string TransType { get; set; }
		
		public virtual string CivilId { get; set; }
		
		public virtual string Image { get; set; }
		
		public virtual double Latitude { get; set; }
		
		public virtual double Longitude { get; set; }
		
		public virtual int SiteId { get; set; }
		
		public virtual string SiteName { get; set; }
		

    }
}