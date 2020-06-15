
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class LocationCredentialDto : EntityDto
    {
		public double Longitude { get; set; }

		public double Latitude { get; set; }


		 public int? LocationId { get; set; }

		 
    }
}