using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllEmployeeWarningsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxFromDateFilter { get; set; }
		public DateTime? MinFromDateFilter { get; set; }

		public DateTime? MaxToDateFilter { get; set; }
		public DateTime? MinToDateFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string WarningTypeNameArFilter { get; set; }

		 
    }
}