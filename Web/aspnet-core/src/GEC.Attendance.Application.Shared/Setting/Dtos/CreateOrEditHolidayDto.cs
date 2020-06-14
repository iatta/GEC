
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditHolidayDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		
		public DateTime StartDate { get; set; }
		
		
		public DateTime EndDate { get; set; }
		
		

    }
}