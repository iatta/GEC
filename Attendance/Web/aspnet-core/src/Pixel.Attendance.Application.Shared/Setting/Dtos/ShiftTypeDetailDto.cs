
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class ShiftTypeDetailDto : EntityDto
    {
		public bool InTimeFirstScan { get; set; }

		public bool InTimeLastScan { get; set; }

		public bool OutTimeFirstScan { get; set; }

		public bool OutTimeLastScan { get; set; }


		 public int ShiftTypeId { get; set; }
		public string SelectedInTime { get; set; }
		public string SelectedOutTime { get; set; }

	}
}