using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetHolidayForEditOutput
    {
		public CreateOrEditHolidayDto Holiday { get; set; }


    }
}