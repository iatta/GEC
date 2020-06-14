
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operations.Dtos
{
    public class TimeProfileDto : EntityDto
    {
		public string DescriptionAr { get; set; }

		public string DescriptionEn { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }


		 public long? UserId { get; set; }

		 
    }
}