using GEC.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace GEC.Attendance.Attendance
{
	[Table("UserDevices")]
    [Audited]
    public class UserDevice : FullAuditedEntity 
    {

		public virtual string DeviceSN { get; set; }
		
		public virtual string LastToken { get; set; }
		
		public virtual string IP { get; set; }
		
		public virtual string OS { get; set; }
		
		public virtual string AppVersion { get; set; }
		
		public virtual string CivilID { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}