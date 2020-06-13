using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetLeaveTypeForEditOutput
    {
		public CreateOrEditLeaveTypeDto LeaveType { get; set; }


    }
}