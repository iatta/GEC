using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllUserTimeSheetApprovesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxMonthFilter { get; set; }
		public int? MinMonthFilter { get; set; }

		public int? MaxYearFilter { get; set; }
		public int? MinYearFilter { get; set; }

		public DateTime? MaxFromDateFilter { get; set; }
		public DateTime? MinFromDateFilter { get; set; }

		public DateTime? MaxToDateFilter { get; set; }
		public DateTime? MinToDateFilter { get; set; }

		public string ApprovedUnitsFilter { get; set; }

		public int ProjectManagerApproveFilter { get; set; }

		public int IsClosedFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string UserName2Filter { get; set; }

		 
    }
}