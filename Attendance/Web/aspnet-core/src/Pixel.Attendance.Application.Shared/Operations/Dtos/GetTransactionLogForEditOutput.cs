using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetTransactionLogForEditOutput
    {
		public CreateOrEditTransactionLogDto TransactionLog { get; set; }

		public string TransactionTransaction_Date { get; set;}

		public string UserName { get; set;}


    }
}