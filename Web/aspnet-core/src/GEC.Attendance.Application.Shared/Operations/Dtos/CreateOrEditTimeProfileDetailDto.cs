
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operations.Dtos
{
    public class CreateOrEditTimeProfileDetailDto : EntityDto<int?>
    {

		public int Day { get; set; }
		
		
		public int ShiftNo { get; set; }
		
		
		 public int? TimeProfileId { get; set; }
		 
		 		 public int? ShiftId { get; set; }
		 
		 
    }
}