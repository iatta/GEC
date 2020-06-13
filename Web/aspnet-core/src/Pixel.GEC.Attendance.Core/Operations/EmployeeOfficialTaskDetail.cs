using Pixel.GEC.Attendance.Operations;
using Pixel.GEC.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Operations
{
	
    public class EmployeeOfficialTaskDetail  
    {
		public virtual int? EmployeeOfficialTaskId { get; set; }
		
		public EmployeeOfficialTask EmployeeOfficialTaskFk { get; set; }
		public virtual long? UserId { get; set; }
		
		public User UserFk { get; set; }
		
    }
}