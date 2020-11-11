using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllTransactionLogsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string OldValueFilter { get; set; }

		public string NewValueFilter { get; set; }


		 public string TransactionTransaction_DateFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}