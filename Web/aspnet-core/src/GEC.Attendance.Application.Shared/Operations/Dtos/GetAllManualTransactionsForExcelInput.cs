using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllManualTransactionsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxTransDateFilter { get; set; }
		public DateTime? MinTransDateFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}