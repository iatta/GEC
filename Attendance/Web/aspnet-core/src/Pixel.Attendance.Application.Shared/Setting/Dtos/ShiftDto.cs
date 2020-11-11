using Pixel.Attendance.Enums;
using System;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class ShiftDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }

		public string Code { get; set; }
		public virtual DayDto DayOff { get; set; }

		public  DayDto DayRest { get; set; }
		public  bool IsDayRestCalculated { get; set; }
		public  int TotalHoursPerDay { get; set; }
		public bool IsInOutWithoutClculateHours { get; set; }

		public  bool IsFlexible { get; set; }
		public  bool IsOneFingerprint { get; set; }
		public  bool IsTwoFingerprint { get; set; }
		public int TimeIn { get; set; }

		public int TimeOut { get; set; }
		public int TotalLateMinutesPerMonth { get; set; }
		public int TotalLateMinutesPerMonthRamadan { get; set; }
		public bool HasRamadanSetting { get; set; }
		public  int TotalHoursPerDayRamadan { get; set; }
		public  int TimeInRamadan { get; set; }
		public  int TimeOutRamadan { get; set; }

		public int EarlyIn { get; set; }

		public int LateIn { get; set; }

		public int EarlyOut { get; set; }

		public int LateOut { get; set; }

		public int TimeInRangeFrom { get; set; }

		public int TimeInRangeTo { get; set; }

		public int TimeOutRangeFrom { get; set; }

		public int TimeOutRangeTo { get; set; }
		public int DeductType { get; set; }
		public bool IsOverTimeAllowed { get; set; }
		public ShiftTypeEnumDto ShiftType { get; set; }


	}
}