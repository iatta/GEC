using Pixel.GEC.Attendance.Enums;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditShiftDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		
		[Required]
		public string Code { get; set; }
		
		
		public DayDto DayOff { get; set; }
		
		
		public DayDto DayRest { get; set; }
		
		
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