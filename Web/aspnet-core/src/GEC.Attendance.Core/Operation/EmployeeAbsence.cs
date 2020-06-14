using GEC.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace GEC.Attendance.Operation
{
	[Table("EmployeeAbsences")]
    public class EmployeeAbsence : FullAuditedEntity 
    {

		public virtual string Note { get; set; }
		
		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}