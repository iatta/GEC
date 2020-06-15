
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditPermitDto : EntityDto<int?>
    {

		[Required]
		public string DescriptionAr { get; set; }
		
		
		[Required]
		public string DescriptionEn { get; set; }

		public  bool LateIn { get; set; }
		public  bool EarlyOut { get; set; }

		public  bool OffShift { get; set; }
		public  bool FullDay { get; set; }

		public  bool InOut { get; set; }


		public int ToleranceIn { get; set; }
		
		
		public int ToleranceOut { get; set; }
		
		
		public int MaxNumberPerDay { get; set; }
		
		
		public string MaxNumberPerWeek { get; set; }
		
		
		public int MaxNumberPerMonth { get; set; }
		
		
		public int TotalHoursPerDay { get; set; }
		
		
		public int TotalHoursPerWeek { get; set; }
		
		
		public int TotalHoursPerMonth { get; set; }
		public bool IsDeducted { get; set; }

		public List<int> SelectedTypesOfPermit { get; set; }

		




	}
}