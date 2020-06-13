﻿
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditLeaveTypeDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		
		[Required]
		public string Code { get; set; }
		
		

    }
}