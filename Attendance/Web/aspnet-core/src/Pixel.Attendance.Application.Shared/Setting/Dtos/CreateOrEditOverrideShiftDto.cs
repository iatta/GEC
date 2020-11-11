
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditOverrideShiftDto : EntityDto<int?>
    {

		public DateTime Day { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? ShiftId { get; set; }
		 
		 
    }
}