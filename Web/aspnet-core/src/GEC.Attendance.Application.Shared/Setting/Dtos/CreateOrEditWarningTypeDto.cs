
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditWarningTypeDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		

    }
}