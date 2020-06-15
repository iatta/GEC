
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class EmployeeVacationDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Description { get; set; }

		public bool Status { get; set; }

		public string Note { get; set; }


		 public long UserId { get; set; }

		 		 public int LeaveTypeId { get; set; }

		 
    }
}