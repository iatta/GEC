
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditTransactionLogDto : EntityDto<int?>
    {

		public string OldValue { get; set; }
		
		
		public string NewValue { get; set; }
		
		
		 public int? TransactionId { get; set; }
		 
		 		 public long? ActionBy { get; set; }
		 
		 
    }
}