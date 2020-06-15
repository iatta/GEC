
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditEmployeePermitDto : EntityDto<int?>
    {

		public int FromTime { get; set; }
		
		
		public int ToTime { get; set; }
		
		
		public DateTime PermitDate { get; set; }

		[Required]
		public string Description { get; set; }
		
		
		public bool Status { get; set; }
		
		
		public string Note { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? PermitId { get; set; }
		 
		 
    }
}