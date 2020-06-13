using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Settings.Dtos
{
    public class GetAllMobileWebPagesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string ContentFilter { get; set; }



    }
}