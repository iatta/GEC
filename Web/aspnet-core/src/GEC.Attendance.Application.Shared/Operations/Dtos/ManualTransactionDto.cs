
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operations.Dtos
{
    public class ManualTransactionDto : EntityDto
    {
		public DateTime TransDate { get; set; }

		public int TransType { get; set; }


		 public long? UserId { get; set; }

		 
    }
}