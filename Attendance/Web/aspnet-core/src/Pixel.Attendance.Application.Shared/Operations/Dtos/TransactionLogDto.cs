
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class TransactionLogDto : EntityDto
    {
		public string OldValue { get; set; }

		public string NewValue { get; set; }


		 public int? TransactionId { get; set; }

		 		 public long? ActionBy { get; set; }
		public DateTime CreationTime { get; set; }
        public bool HasDifferent { get; set; }



    }
}