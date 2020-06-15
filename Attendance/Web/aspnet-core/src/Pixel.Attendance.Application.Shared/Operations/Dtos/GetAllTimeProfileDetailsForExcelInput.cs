using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllTimeProfileDetailsForExcelInput
    {
		public string Filter { get; set; }


		 public string TimeProfileDescriptionArFilter { get; set; }

		 		 public string ShiftNameArFilter { get; set; }

		 
    }
}