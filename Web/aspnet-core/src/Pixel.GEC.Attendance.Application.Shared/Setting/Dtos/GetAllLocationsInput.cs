using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllLocationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleArFilter { get; set; }

		public string TitleEnFilter { get; set; }



    }
}