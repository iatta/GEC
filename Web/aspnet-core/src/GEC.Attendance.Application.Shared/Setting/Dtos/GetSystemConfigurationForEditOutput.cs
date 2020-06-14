using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetSystemConfigurationForEditOutput
    {
		public CreateOrEditSystemConfigurationDto SystemConfiguration { get; set; }


    }
}