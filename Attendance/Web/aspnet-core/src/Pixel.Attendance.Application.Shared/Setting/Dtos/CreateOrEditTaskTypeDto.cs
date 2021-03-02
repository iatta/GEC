
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditTaskTypeDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		
		[Required]
		public string Number { get; set; }
		
		

    }
}