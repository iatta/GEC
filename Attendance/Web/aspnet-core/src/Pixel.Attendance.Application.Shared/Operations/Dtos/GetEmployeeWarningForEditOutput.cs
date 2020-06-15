using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetEmployeeWarningForEditOutput
    {
		public CreateOrEditEmployeeWarningDto EmployeeWarning { get; set; }

		public string UserName { get; set;}

		public string WarningTypeNameAr { get; set;}


    }
}