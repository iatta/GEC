using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Settings.Dtos
{
    public class GetAllMobileWebPagesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string ContentFilter { get; set; }



    }
}