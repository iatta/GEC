
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditOfficialTaskTypeDto : EntityDto<int?>
    {

		public string NameAr { get; set; }
		
		
		public string NameEn { get; set; }
		
		
		public bool TypeIn { get; set; }
		
		
		public bool TypeOut { get; set; }
		
		
		public bool TypeInOut { get; set; }
		
		

    }
}