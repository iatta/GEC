using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllEmployeeOfficialTasksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxFromDateFilter { get; set; }
		public DateTime? MinFromDateFilter { get; set; }

		public DateTime? MaxToDateFilter { get; set; }
		public DateTime? MinToDateFilter { get; set; }

		public string RemarksFilter { get; set; }

		public string DescriptionArFilter { get; set; }

		public string DescriptionEnFilter { get; set; }


		 public string OfficialTaskTypeNameArFilter { get; set; }

		 
    }
}