using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Setting
{
	[Table("TempTransactions")]
    public class TempTransaction : Entity 
    {

		public virtual DateTime Date { get; set; }
		
		public virtual int Pin { get; set; }
		
		public virtual string In { get; set; }
		
		public virtual DateTime? Timesheet_in { get; set; }
		
		public virtual string Out { get; set; }
		
		public virtual DateTime? Timesheet_out { get; set; }
		
		public virtual string LI { get; set; }
		
		public virtual string EO { get; set; }
		
		public virtual int PermissionLI_ID { get; set; }
		
		public virtual int PermissionEO_ID { get; set; }
		
		public virtual int PermissionIO_ID { get; set; }
		
		public virtual string Permission_Text { get; set; }
		
		public virtual int OfficialTask_In_ID { get; set; }
		
		public virtual int OfficialTask_Out_ID { get; set; }
		
		public virtual int OfficialTask_InOut_ID { get; set; }
		
		public virtual string OfficialTask_Text { get; set; }
		
		public virtual DateTime TimeSheet_ExactOut { get; set; }
		
		public virtual int ShiftID { get; set; }
		
		public virtual int OverTime { get; set; }
		
		public virtual int Exception_In { get; set; }
		
		public virtual int Exception_out { get; set; }
		
		public virtual int TimeProfileID { get; set; }
		
		public virtual int ShiftType { get; set; }
		
		public virtual bool isNight { get; set; }
		

    }
}