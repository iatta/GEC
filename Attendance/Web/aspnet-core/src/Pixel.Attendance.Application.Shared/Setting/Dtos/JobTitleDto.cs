
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class JobTitleDto : EntityDto
    {
		public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }


    }
}