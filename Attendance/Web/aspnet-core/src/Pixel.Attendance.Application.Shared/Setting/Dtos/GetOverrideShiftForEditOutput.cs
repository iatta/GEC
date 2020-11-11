using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetOverrideShiftForEditOutput
    {
		public CreateOrEditOverrideShiftDto OverrideShift { get; set; }

		public string UserName { get; set;}

		public string ShiftNameEn { get; set;}


    }
}