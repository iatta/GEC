using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllTransactionsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxTransTypeFilter { get; set; }
		public int? MinTransTypeFilter { get; set; }



    }
}