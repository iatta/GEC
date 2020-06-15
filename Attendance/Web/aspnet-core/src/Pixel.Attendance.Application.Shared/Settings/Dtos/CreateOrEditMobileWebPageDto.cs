
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Settings.Dtos
{
    public class CreateOrEditMobileWebPageDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Content { get; set; }
		
		

    }
}