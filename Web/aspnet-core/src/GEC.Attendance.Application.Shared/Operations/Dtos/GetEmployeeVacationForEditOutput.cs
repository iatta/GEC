using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetEmployeeVacationForEditOutput
    {
		public CreateOrEditEmployeeVacationDto EmployeeVacation { get; set; }

		public string UserName { get; set;}

		public string LeaveTypeNameAr { get; set;}


    }
}