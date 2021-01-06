
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class RamadanDateDto : EntityDto
    {
		public string Year { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }



    }
}