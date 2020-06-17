using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllUserShiftsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxDateFilter { get; set; }
		public DateTime? MinDateFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string ShiftNameEnFilter { get; set; }

		 
    }
}