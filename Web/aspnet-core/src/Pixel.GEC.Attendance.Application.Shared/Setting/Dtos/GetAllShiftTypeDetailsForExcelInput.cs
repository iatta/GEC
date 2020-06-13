using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllShiftTypeDetailsForExcelInput
    {
		public string Filter { get; set; }


		 public string ShiftTypeDescriptionArFilter { get; set; }

		 
    }
}