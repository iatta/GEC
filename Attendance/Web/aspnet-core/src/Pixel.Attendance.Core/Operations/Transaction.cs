using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;

namespace Pixel.Attendance.Operations
{
	[Table("Transactions")]
    public class Transaction : FullAuditedEntity 
    {

		public virtual string Time { get; set; }
		
		public virtual int SerialNo { get; set; }
		
		public virtual int TransType { get; set; }
		
		public virtual long Pin { get; set; }

		[ForeignKey("Pin")]
		public User User { get; set; }
		public virtual int Finger { get; set; }
		
		public virtual string Address { get; set; }
		
		public virtual int Manual { get; set; }
		
		public virtual int ScanType { get; set; }
		
		public virtual bool IsProcessed { get; set; }
		
		public virtual bool IsException { get; set; }
		
		public virtual string Reason { get; set; }
		
		public virtual int TimeSheetID { get; set; }
		
		public virtual int ProcessInOut { get; set; }
		
		public virtual string Remark { get; set; }
		
		public virtual int KeyType { get; set; }
		
		public virtual DateTime ImportDate { get; set; }
		
		public virtual DateTime Transaction_Date { get; set; }

		public bool ProjectManagerApprove { get; set; }
		public bool UnitManagerApprove { get; set; }
		public bool HrApprove { get; set; }
		public int MachineId { get; set; }

		[ForeignKey("MachineId")]
		public Machine Machine { get; set; }
        public bool IsTaken { get; set; }
        public string EditNote { get; set; }

    }
}