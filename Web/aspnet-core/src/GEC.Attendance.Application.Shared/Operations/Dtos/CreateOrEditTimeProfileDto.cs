
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GEC.Attendance.Operations.Dtos
{
    public class CreateOrEditTimeProfileDto : EntityDto<int?>
    {

		public CreateOrEditTimeProfileDto()
		{
			TimeProfileDetails = new List<TimeProfileDetailDto>();
		}
		public string DescriptionAr { get; set; }
		
		
		public string DescriptionEn { get; set; }
		
		
		public DateTime StartDate { get; set; }
		
		
		public DateTime EndDate { get; set; }
		
		
		public int ShiftTypeID_Saturday { get; set; }
		
		
		public int ShiftTypeID_Sunday { get; set; }
		
		
		public int ShiftTypeID_Monday { get; set; }
		
		
		public int ShiftTypeID_Tuesday { get; set; }
		
		
		public int ShiftTypeID_Wednesday { get; set; }
		
		
		public int ShiftTypeID_Thursday { get; set; }
		
		
		public int ShiftTypeID_Friday { get; set; }
		
		
		 public long? UserId { get; set; }

		public List<TimeProfileDetailDto> TimeProfileDetails { get; set; }

		public int ShiftId { get; set; }


	}
}