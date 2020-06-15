using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetTempTransactionForEditOutput
    {
		public CreateOrEditTempTransactionDto TempTransaction { get; set; }


    }
}