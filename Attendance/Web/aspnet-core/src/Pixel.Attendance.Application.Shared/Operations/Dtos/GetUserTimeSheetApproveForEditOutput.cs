using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetUserTimeSheetApproveForEditOutput
    {
		public CreateOrEditUserTimeSheetApproveDto UserTimeSheetApprove { get; set; }

		public string UserName { get; set;}

		public string ProjectManagerName { get; set;}


    }
}