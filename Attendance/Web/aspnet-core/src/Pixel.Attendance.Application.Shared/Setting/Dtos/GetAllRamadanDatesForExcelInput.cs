using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllRamadanDatesForExcelInput
    {
		public string Filter { get; set; }

		public string YearFilter { get; set; }

		public DateTime? MaxFromDateFilter { get; set; }
		public DateTime? MinFromDateFilter { get; set; }

		public DateTime? MaxToDateFilter { get; set; }
		public DateTime? MinToDateFilter { get; set; }



    }
}