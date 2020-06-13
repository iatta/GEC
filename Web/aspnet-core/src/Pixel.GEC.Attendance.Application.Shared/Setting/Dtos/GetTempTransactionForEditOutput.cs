using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetTempTransactionForEditOutput
    {
		public CreateOrEditTempTransactionDto TempTransaction { get; set; }


    }
}