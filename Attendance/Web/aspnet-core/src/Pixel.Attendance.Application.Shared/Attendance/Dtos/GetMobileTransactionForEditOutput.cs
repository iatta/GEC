using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Attendance.Dtos
{
    public class GetMobileTransactionForEditOutput
    {
		public CreateOrEditMobileTransactionDto MobileTransaction { get; set; }


    }
}