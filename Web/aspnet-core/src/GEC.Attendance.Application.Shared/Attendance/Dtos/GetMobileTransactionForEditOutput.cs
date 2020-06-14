using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Attendance.Dtos
{
    public class GetMobileTransactionForEditOutput
    {
		public CreateOrEditMobileTransactionDto MobileTransaction { get; set; }


    }
}