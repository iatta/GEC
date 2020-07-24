
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditManualTransactionDto : EntityDto<int?>
    {

		public DateTime TransDate { get; set; }
		
		
		public int TransType { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? MachineId { get; set; }
		 
		 
    }
}