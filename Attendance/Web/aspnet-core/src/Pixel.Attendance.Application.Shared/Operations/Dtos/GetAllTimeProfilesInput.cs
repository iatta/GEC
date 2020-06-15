using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllTimeProfilesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionArFilter { get; set; }

		public string DescriptionEnFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}