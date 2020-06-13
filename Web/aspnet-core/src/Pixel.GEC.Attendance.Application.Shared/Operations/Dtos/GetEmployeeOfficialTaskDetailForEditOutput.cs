using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetEmployeeOfficialTaskDetailForEditOutput
    {
		public CreateOrEditEmployeeOfficialTaskDetailDto EmployeeOfficialTaskDetail { get; set; }

		public string EmployeeOfficialTaskDescriptionAr { get; set;}

		public string UserName { get; set;}


    }
}