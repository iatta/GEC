
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditLocationDto : EntityDto<int?>
    {

		public string TitleAr { get; set; }
		
		
		public string TitleEn { get; set; }

		public ICollection<LocationCredentialDto> LocationCredentials { get; set; }



	}
}