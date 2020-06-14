
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Setting.Dtos
{
    public class OfficialTaskTypeDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }

		public bool TypeIn { get; set; }

		public bool TypeOut { get; set; }

		public bool TypeInOut { get; set; }



    }
}