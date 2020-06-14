using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetManualTransactionForEditOutput
    {
		public CreateOrEditManualTransactionDto ManualTransaction { get; set; }

		public string UserName { get; set;}


    }
}