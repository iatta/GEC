using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operation.Dtos
{
    public class GetEmployeeAbsenceForEditOutput
    {
		public CreateOrEditEmployeeAbsenceDto EmployeeAbsence { get; set; }

		public string UserName { get; set;}


    }
}