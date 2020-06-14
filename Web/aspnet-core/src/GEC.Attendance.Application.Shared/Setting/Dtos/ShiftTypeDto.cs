
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Setting.Dtos
{
    public class ShiftTypeDto : EntityDto
    {
		public string DescriptionEn { get; set; }

		public string DescriptionAr { get; set; }

		public int NumberOfDuties { get; set; }

		public bool InScan { get; set; }

		public bool OutScan { get; set; }

		public bool CrossDay { get; set; }

		public bool AlwaysAttend { get; set; }

		public bool Open { get; set; }

		public int MaxBoundryTime { get; set; }



    }
}