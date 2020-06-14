
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Attendance.Dtos
{
    public class CreateOrEditUserDeviceDto : EntityDto<int?>
    {

		public string DeviceSN { get; set; }
		
		
		public string LastToken { get; set; }
		
		
		public string IP { get; set; }
		
		
		public string OS { get; set; }
		
		
		public string AppVersion { get; set; }
		
		
		public string CivilID { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}