using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Attendance.Dtos
{
    public class GetAllUserDevicesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}