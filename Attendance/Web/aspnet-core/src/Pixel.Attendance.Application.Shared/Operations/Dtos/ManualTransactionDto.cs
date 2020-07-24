
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class ManualTransactionDto : EntityDto
    {
		public DateTime TransDate { get; set; }

		public int TransType { get; set; }


		 public long? UserId { get; set; }

		 		 public int? MachineId { get; set; }

		 
    }
}