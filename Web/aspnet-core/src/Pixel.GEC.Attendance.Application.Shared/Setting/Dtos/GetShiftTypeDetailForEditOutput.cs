using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetShiftTypeDetailForEditOutput
    {
		public CreateOrEditShiftTypeDetailDto ShiftTypeDetail { get; set; }

		public string ShiftTypeDescriptionAr { get; set;}


    }
}