using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Attendance.Dtos
{
    public class GetAllMobileTransactionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxSiteIdFilter { get; set; }
		public int? MinSiteIdFilter { get; set; }

		public string SiteNameFilter { get; set; }



    }
}