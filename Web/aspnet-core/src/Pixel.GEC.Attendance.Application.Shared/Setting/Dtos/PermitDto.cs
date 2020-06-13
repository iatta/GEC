
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class PermitDto : EntityDto
    {
		public string DescriptionAr { get; set; }

		public string DescriptionEn { get; set; }
		public bool LateIn { get; set; }
		public bool EarlyOut { get; set; }

		public bool OffShift { get; set; }
		public bool FullDay { get; set; }

		public bool InOut { get; set; }


		public int? ToleranceIn { get; set; }

		public int? ToleranceOut { get; set; }

		public int? MaxNumberPerDay { get; set; }

		public int? MaxNumberPerWeek { get; set; }

		public int? MaxNumberPerMonth { get; set; }

		public int? TotalHoursPerDay { get; set; }

		public int? TotalHoursPerWeek { get; set; }

		public int? TotalHoursPerMonth { get; set; }

		
		public bool IsDeducted { get; set; }



    }
}