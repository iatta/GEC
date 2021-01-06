
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditRamadanDateDto : EntityDto<int?>
    {

		[Required]
		public string Year { get; set; }
		
		
		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		

    }
}