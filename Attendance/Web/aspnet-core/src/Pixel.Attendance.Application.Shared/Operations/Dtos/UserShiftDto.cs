
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;

namespace Pixel.Attendance.Operations.Dtos
{
    public class UserShiftDto : EntityDto
    {
		public DateTime Date { get; set; }


		 public long? UserId { get; set; }

		public int? ShiftId { get; set; }

		public ShiftDto Shift { get; set; }
		[NotMapped]
		public bool IsNew { get; set; }
		public bool IsModified { get; set; }
		public bool IsDeleted { get; set; }

	}
}