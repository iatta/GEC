
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operation.Dtos
{
    public class EmployeeAbsenceDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }


		 public long? UserId { get; set; }

		 
    }
}