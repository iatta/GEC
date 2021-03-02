using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllTransactionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTransTypeFilter { get; set; }
		public int? MinTransTypeFilter { get; set; }

        public string UserNameFilter { get; set; }
        public DateTime? MaxTransDateFilter { get; set; }
        public DateTime? MinTransDateFilter { get; set; }
        public string MachineNameEnFilter { get; set; }


    }
}