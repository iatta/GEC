using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllLocationsForExcelInput
    {
		public string Filter { get; set; }

		public string TitleArFilter { get; set; }

		public string TitleEnFilter { get; set; }



    }
}