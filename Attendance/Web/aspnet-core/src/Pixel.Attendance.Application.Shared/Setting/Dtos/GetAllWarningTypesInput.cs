using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllWarningTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }



    }
}