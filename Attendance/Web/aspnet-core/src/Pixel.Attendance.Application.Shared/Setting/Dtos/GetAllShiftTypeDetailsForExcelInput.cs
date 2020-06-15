using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllShiftTypeDetailsForExcelInput
    {
		public string Filter { get; set; }


		 public string ShiftTypeDescriptionArFilter { get; set; }

		 
    }
}