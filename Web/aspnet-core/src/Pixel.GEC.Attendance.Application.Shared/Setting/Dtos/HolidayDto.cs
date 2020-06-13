
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class HolidayDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }



    }
}