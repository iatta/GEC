using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllOverrideShiftsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxDayFilter { get; set; }
		public DateTime? MinDayFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string ShiftNameEnFilter { get; set; }

		 
    }
}