using Pixel.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Operations
{
	[Table("Trans")]
    public class Tran : FullAuditedEntity 
    {

		public virtual string Scan1 { get; set; }
		
		public virtual string Scan2 { get; set; }
		
		public virtual string Scan3 { get; set; }
		
		public virtual string Scan4 { get; set; }
		
		public virtual string Scan5 { get; set; }
		
		public virtual string Scan6 { get; set; }
		
		public virtual string Scan8 { get; set; }
		
		public virtual string ScanLocation1 { get; set; }
		
		public virtual string ScanLocation2 { get; set; }
		
		public virtual string ScanLocation3 { get; set; }
		
		public virtual string ScanLocation4 { get; set; }
		
		public virtual string ScanLocation5 { get; set; }
		
		public virtual string ScanLocation6 { get; set; }
		
		public virtual string ScanLocation7 { get; set; }
		
		public virtual string ScanLocation8 { get; set; }
		
		public virtual bool HasHoliday { get; set; }
		
		public virtual bool HasVacation { get; set; }
		
		public virtual bool HasOffDay { get; set; }
		
		public virtual bool IsAbsent { get; set; }
		
		public virtual string LeaveCode { get; set; }
		
		public virtual int DesignationID { get; set; }
		
		public virtual string LeaveRemark { get; set; }
		
		public virtual int NoShifts { get; set; }
		
		public virtual string ShiftName { get; set; }
		
		public virtual bool ScanManual1 { get; set; }
		
		public virtual bool ScanManual2 { get; set; }
		
		public virtual bool ScanManual3 { get; set; }
		
		public virtual bool ScanManual4 { get; set; }
		
		public virtual bool ScanManual5 { get; set; }
		
		public virtual bool ScanManual6 { get; set; }
		
		public virtual bool ScanManual7 { get; set; }
		
		public virtual bool ScanManual8 { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}