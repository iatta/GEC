
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditUserShiftDto : EntityDto<int?>
    {

		public DateTime Date { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 public int? ShiftId { get; set; }
		 
		 
    }
}