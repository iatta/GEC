using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetShiftForEditOutput
    {
		public CreateOrEditShiftDto Shift { get; set; }


    }
}