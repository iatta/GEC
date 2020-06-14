using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllLocationCredentialsForExcelInput
    {
		public string Filter { get; set; }

		public double? MaxLongitudeFilter { get; set; }
		public double? MinLongitudeFilter { get; set; }

		public double? MaxLatitudeFilter { get; set; }
		public double? MinLatitudeFilter { get; set; }


		 public string LocationDescriptionArFilter { get; set; }

		 
    }
}