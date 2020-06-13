
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class CreateOrEditEmployeeWarningDto : EntityDto<int?>
    {

		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? WarningTypeId { get; set; }
		 
		 
    }
}