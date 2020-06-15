
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class TranDto : EntityDto
    {
		public string Scan1 { get; set; }

		public string Scan2 { get; set; }

		public string Scan3 { get; set; }

		public string Scan4 { get; set; }

		public string Scan5 { get; set; }

		public string Scan6 { get; set; }

		public string Scan8 { get; set; }

		public string ScanLocation1 { get; set; }

		public string ScanLocation2 { get; set; }

		public string ScanLocation3 { get; set; }

		public string ScanLocation4 { get; set; }

		public string ScanLocation5 { get; set; }

		public string ScanLocation6 { get; set; }

		public string ScanLocation7 { get; set; }

		public string ScanLocation8 { get; set; }

		public bool HasHoliday { get; set; }

		public bool HasVacation { get; set; }

		public bool HasOffDay { get; set; }

		public bool IsAbsent { get; set; }

		public string LeaveCode { get; set; }

		public int DesignationID { get; set; }

		public string LeaveRemark { get; set; }

		public int NoShifts { get; set; }

		public string ShiftName { get; set; }

		public bool ScanManual1 { get; set; }

		public bool ScanManual2 { get; set; }

		public bool ScanManual3 { get; set; }

		public bool ScanManual4 { get; set; }

		public bool ScanManual5 { get; set; }

		public bool ScanManual6 { get; set; }

		public bool ScanManual7 { get; set; }

		public bool ScanManual8 { get; set; }


		 public long? UserId { get; set; }

		 
    }
}