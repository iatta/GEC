using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Attendance.Dtos
{
    public class GetUserDeviceForEditOutput
    {
		public CreateOrEditUserDeviceDto UserDevice { get; set; }

		public string UserName { get; set;}


    }
}