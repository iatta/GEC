using Pixel.Attendance.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
	[Table("TimeProfiles")]
    public class TimeProfile : FullAuditedEntity 
    {
		public TimeProfile()
		{
			TimeProfileDetails = new HashSet<TimeProfileDetail>();
		}

		public virtual string DescriptionAr { get; set; }
		
		public virtual string DescriptionEn { get; set; }
		
		public virtual DateTime StartDate { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		
		public virtual int ShiftTypeID_Saturday { get; set; }
		
		public virtual int ShiftTypeID_Sunday { get; set; }
		
		public virtual int ShiftTypeID_Monday { get; set; }
		
		public virtual int ShiftTypeID_Tuesday { get; set; }
		
		public virtual int ShiftTypeID_Wednesday { get; set; }
		
		public virtual int ShiftTypeID_Thursday { get; set; }
		
		public virtual int ShiftTypeID_Friday { get; set; }

		public virtual ICollection<TimeProfileDetail> TimeProfileDetails { get; set; }


		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}