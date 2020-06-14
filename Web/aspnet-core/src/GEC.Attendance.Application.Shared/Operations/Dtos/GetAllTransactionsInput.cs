using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllTransactionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTransTypeFilter { get; set; }
		public int? MinTransTypeFilter { get; set; }



    }
}