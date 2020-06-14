using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllHolidaysForExcelInput
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }

		public string NameEnFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }



    }
}