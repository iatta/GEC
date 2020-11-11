using Pixel.Attendance.Operations;
using Pixel.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("TransactionLogs")]
    [Audited]
    public class TransactionLog : FullAuditedEntity 
    {

		public virtual string OldValue { get; set; }
		
		public virtual string NewValue { get; set; }
		

		public virtual int? TransactionId { get; set; }
		
        [ForeignKey("TransactionId")]
		public Transaction TransactionFk { get; set; }
		
		public virtual long? ActionBy { get; set; }
		
        [ForeignKey("ActionBy")]
		public User ActionFk { get; set; }
		
    }
}