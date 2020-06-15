using Pixel.Attendance.Enums;
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class ShiftDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }

		public string Code { get; set; }

		public int TimeIn { get; set; }

		public int TimeOut { get; set; }

		public int EarlyIn { get; set; }

		public int LateIn { get; set; }

		public int EarlyOut { get; set; }

		public int LateOut { get; set; }

		public int TimeInRangeFrom { get; set; }

		public int TimeInRangeTo { get; set; }

		public int TimeOutRangeFrom { get; set; }

		public int TimeOutRangeTo { get; set; }
		public int DeductType { get; set; }


	}
}