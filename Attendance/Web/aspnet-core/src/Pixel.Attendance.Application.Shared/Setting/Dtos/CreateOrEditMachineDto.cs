
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditMachineDto : EntityDto<int?>
    {

		public string NameAr { get; set; }
		
		
		public string NameEn { get; set; }
		
		
		public string IpAddress { get; set; }
		
		
		public string SubNet { get; set; }
		
		
		public bool Status { get; set; }
		
		
		public string Port { get; set; }
		
		
		public bool Online { get; set; }
		
		
		 public long? OrganizationUnitId { get; set; }
		 
		 
    }
}