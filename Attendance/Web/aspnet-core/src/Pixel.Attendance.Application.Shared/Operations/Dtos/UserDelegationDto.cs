
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class UserDelegationDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }


		 public long? UserId { get; set; }

		 		 public long? DelegatedUserId { get; set; }

		 
    }
}