using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllSystemConfigurationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}