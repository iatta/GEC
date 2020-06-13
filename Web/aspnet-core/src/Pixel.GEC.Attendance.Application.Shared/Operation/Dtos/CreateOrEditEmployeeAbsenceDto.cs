
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operation.Dtos
{
    public class CreateOrEditEmployeeAbsenceDto : EntityDto<int?>
    {

		public string Note { get; set; }
		
		
		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}