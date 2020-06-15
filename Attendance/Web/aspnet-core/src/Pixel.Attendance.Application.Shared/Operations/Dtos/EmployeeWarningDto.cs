
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class EmployeeWarningDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }


		 public long? UserId { get; set; }

		 		 public int? WarningTypeId { get; set; }

		 
    }
}