using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Attendance.Dtos
{
    public class GetAllMobileTransactionsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSiteIdFilter { get; set; }
		public int? MinSiteIdFilter { get; set; }

		public string SiteNameFilter { get; set; }



    }
}