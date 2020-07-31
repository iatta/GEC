using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("UserTimeSheetApproves")]
    [Audited]
    public class UserTimeSheetApprove : FullAuditedEntity 
    {

		public virtual int Month { get; set; }
		
		public virtual int Year { get; set; }
		
		public virtual DateTime? FromDate { get; set; }
		
		public virtual DateTime? ToDate { get; set; }
		
		public virtual string ApprovedUnits { get; set; }
		
		public virtual bool ProjectManagerApprove { get; set; }
		
		public virtual bool IsClosed { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual long? ProjectManagerId { get; set; }
		
        [ForeignKey("ProjectManagerId")]
		public User ProjectManagerFk { get; set; }
		
		public virtual int? ProjectId { get; set; }
		
        [ForeignKey("ProjectId")]
		public Project ProjectFk { get; set; }
		
    }
}