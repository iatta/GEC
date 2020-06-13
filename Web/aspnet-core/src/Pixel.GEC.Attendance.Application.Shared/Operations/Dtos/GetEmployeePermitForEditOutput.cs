using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetEmployeePermitForEditOutput
    {
		public CreateOrEditEmployeePermitDto EmployeePermit { get; set; }

		public string UserName { get; set;}

		public string PermitDescriptionAr { get; set;}


    }
}