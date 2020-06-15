using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllJobTitlesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }
        public string NameEnFilter { get; set; }



    }
}