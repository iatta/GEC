
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditNationalityDto : EntityDto<int?>
    {

		public string NameAr { get; set; }
		
		
		public string NameEn { get; set; }
		
		

    }
}