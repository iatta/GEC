
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditUserDelegationDto : EntityDto<int?>
    {

		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public long? DelegatedUserId { get; set; }
		 
		 
    }
}