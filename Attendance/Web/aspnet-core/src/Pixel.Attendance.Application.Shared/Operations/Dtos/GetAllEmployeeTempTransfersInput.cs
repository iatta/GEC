using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllEmployeeTempTransfersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxFromDateFilter { get; set; }
		public DateTime? MinFromDateFilter { get; set; }

		public DateTime? MaxToDateFilter { get; set; }
		public DateTime? MinToDateFilter { get; set; }


		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}