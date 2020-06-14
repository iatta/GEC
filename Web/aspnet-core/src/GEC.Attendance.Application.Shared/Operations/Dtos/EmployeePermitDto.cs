
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operations.Dtos
{
    public class EmployeePermitDto : EntityDto
    {
		public int FromTime { get; set; }
		public int ToTime { get; set; }

		public DateTime PermitDate { get; set; }

		public string Description { get; set; }

		public bool Status { get; set; }


		 public long? UserId { get; set; }

		public int? PermitId { get; set; }

		 
    }
}