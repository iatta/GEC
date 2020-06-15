using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllShiftsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }

		public string NameEnFilter { get; set; }

		public string CodeFilter { get; set; }



    }
}