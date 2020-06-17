
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class UserShiftDto : EntityDto
    {
		public DateTime Date { get; set; }


		 public long? UserId { get; set; }

		 		 public int? ShiftId { get; set; }

		 
    }
}