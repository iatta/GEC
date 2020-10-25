using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetEmployeeTempTransferForEditOutput
    {
		public CreateOrEditEmployeeTempTransferDto EmployeeTempTransfer { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string UserName { get; set;}


    }
}