using GEC.Attendance.Operations;
using GEC.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace GEC.Attendance.Operations
{
	
    public class EmployeeOfficialTaskDetail  
    {
		public virtual int? EmployeeOfficialTaskId { get; set; }
		
		public EmployeeOfficialTask EmployeeOfficialTaskFk { get; set; }
		public virtual long? UserId { get; set; }
		
		public User UserFk { get; set; }
		
    }
}