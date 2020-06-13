
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class SystemConfigurationDto : EntityDto
    {
		public int TotalPermissionNumberPerMonth { get; set; }

		public int TotalPermissionNumberPerWeek { get; set; }

		public int TotalPermissionNumberPerDay { get; set; }

		public int TotalPermissionHoursPerMonth { get; set; }

		public int TotalPermissionHoursPerWeek { get; set; }

		public int TotalPermissionHoursPerDay { get; set; }



    }
}