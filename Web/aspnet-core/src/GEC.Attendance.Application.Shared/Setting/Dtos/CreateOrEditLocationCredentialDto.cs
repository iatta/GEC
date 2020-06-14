
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditLocationCredentialDto : EntityDto<int?>
    {

		public double Longitude { get; set; }
		
		
		public double Latitude { get; set; }
		
		
		 public int? LocationId { get; set; }
		 
		 
    }
}