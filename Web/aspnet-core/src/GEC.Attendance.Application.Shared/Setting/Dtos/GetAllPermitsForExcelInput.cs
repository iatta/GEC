using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllPermitsForExcelInput
    {
		public string Filter { get; set; }

		public string DescriptionArFilter { get; set; }

		public string DescriptionEnFilter { get; set; }



    }
}