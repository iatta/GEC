using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetAllTransForExcelInput
    {
		public string Filter { get; set; }

		public string Scan1Filter { get; set; }

		public string Scan2Filter { get; set; }

		public string Scan3Filter { get; set; }

		public string Scan4Filter { get; set; }

		public string Scan5Filter { get; set; }

		public string Scan6Filter { get; set; }

		public string Scan8Filter { get; set; }

		public string ScanLocation1Filter { get; set; }

		public string ScanLocation2Filter { get; set; }

		public string ScanLocation3Filter { get; set; }

		public string ScanLocation4Filter { get; set; }

		public string ScanLocation5Filter { get; set; }

		public string ScanLocation6Filter { get; set; }

		public string ScanLocation7Filter { get; set; }

		public string ScanLocation8Filter { get; set; }

		public int HasHolidayFilter { get; set; }

		public int HasVacationFilter { get; set; }

		public int HasOffDayFilter { get; set; }

		public int IsAbsentFilter { get; set; }

		public string LeaveCodeFilter { get; set; }

		public int? MaxDesignationIDFilter { get; set; }
		public int? MinDesignationIDFilter { get; set; }

		public string LeaveRemarkFilter { get; set; }

		public int? MaxNoShiftsFilter { get; set; }
		public int? MinNoShiftsFilter { get; set; }

		public string ShiftNameFilter { get; set; }

		public int ScanManual1Filter { get; set; }

		public int ScanManual2Filter { get; set; }

		public int ScanManual3Filter { get; set; }

		public int ScanManual4Filter { get; set; }

		public int ScanManual5Filter { get; set; }

		public int ScanManual6Filter { get; set; }

		public int ScanManual7Filter { get; set; }

		public int ScanManual8Filter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}