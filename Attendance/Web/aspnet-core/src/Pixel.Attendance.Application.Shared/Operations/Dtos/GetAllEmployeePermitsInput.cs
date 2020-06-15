using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllEmployeePermitsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxPermitDateFilter { get; set; }
		public DateTime? MinPermitDateFilter { get; set; }

		public int StatusFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string PermitDescriptionArFilter { get; set; }

		 
    }
}