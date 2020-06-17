using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetUserShiftForEditOutput
    {
		public CreateOrEditUserShiftDto UserShift { get; set; }

		public string UserName { get; set;}

		public string ShiftNameEn { get; set;}


    }
}