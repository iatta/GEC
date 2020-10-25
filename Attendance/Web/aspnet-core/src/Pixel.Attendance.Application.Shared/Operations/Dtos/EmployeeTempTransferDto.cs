
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class EmployeeTempTransferDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }


		 public long? OrganizationUnitId { get; set; }

		 		 public long? UserId { get; set; }

		 
    }
}