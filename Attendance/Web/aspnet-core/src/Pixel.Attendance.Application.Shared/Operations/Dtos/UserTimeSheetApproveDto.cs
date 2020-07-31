
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class UserTimeSheetApproveDto : EntityDto
    {
		public int Month { get; set; }

		public int Year { get; set; }

		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }

		public string ApprovedUnits { get; set; }

		public bool ProjectManagerApprove { get; set; }

		public bool IsClosed { get; set; }


		 public long? UserId { get; set; }

		 		 public long? ProjectManagerId { get; set; }

		 		 public int? ProjectId { get; set; }

		 
    }
}