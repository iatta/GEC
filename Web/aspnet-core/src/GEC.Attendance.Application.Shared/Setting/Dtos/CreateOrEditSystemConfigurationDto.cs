
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditSystemConfigurationDto : EntityDto<int?>
    {

		public int TotalPermissionNumberPerMonth { get; set; }
		
		
		public int TotalPermissionNumberPerWeek { get; set; }
		
		
		public int TotalPermissionNumberPerDay { get; set; }
		
		
		public int TotalPermissionHoursPerMonth { get; set; }
		
		
		public int TotalPermissionHoursPerWeek { get; set; }
		
		
		public int TotalPermissionHoursPerDay { get; set; }
		
		

    }
}