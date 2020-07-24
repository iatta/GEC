using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Operations
{
	[Table("ManualTransactions")]
    public class ManualTransaction : FullAuditedEntity 
    {

		public virtual string FingerCode { get; set; }
		
		public virtual DateTime TransDate { get; set; }
		
		public virtual int TransType { get; set; }
		
		public virtual string CivilId { get; set; }
		
		public virtual string Image { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? MachineId { get; set; }
		
        [ForeignKey("MachineId")]
		public Machine MachineFk { get; set; }
		
    }
}