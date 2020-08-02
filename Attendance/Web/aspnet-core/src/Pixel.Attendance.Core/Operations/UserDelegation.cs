using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Pixel.Attendance.Operations
{
	[Table("UserDelegations")]
    [Audited]
    public class UserDelegation : Entity 
    {

		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual long? DelegatedUserId { get; set; }
		
        [ForeignKey("DelegatedUserId")]
		public User DelegatedUserFk { get; set; }
		
    }
}