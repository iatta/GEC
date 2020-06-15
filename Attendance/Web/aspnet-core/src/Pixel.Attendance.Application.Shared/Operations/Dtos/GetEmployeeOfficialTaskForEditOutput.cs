using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetEmployeeOfficialTaskForEditOutput
    {
		public CreateOrEditEmployeeOfficialTaskDto EmployeeOfficialTask { get; set; }

		public string OfficialTaskTypeNameAr { get; set;}


    }
}