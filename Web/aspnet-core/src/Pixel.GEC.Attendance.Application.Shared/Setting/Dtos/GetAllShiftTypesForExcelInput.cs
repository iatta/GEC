using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllShiftTypesForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionEnFilter { get; set; }

		public string DescriptionArFilter { get; set; }



    }
}