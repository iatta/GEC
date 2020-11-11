
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class OverrideShiftDto : EntityDto
    {
		public DateTime Day { get; set; }


		 public long? UserId { get; set; }

		 		 public int? ShiftId { get; set; }

		 
    }
}