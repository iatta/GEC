
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class TimeProfileDetailDto : EntityDto
    {

		 public int? TimeProfileId { get; set; }

		 public int? ShiftId { get; set; }
        public int Day { get; set; }


    }
}