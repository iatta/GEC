
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Attendance.Dtos
{
    public class CreateOrEditMobileTransactionDto : EntityDto<int?>
    {

		public string FingerCode { get; set; }
		
		
		public string MachineID { get; set; }
		
		
		public bool TransStatus { get; set; }
		
		
		public DateTime TransDate { get; set; }
		
		
		public int TransType { get; set; }
		
		
		public string CivilId { get; set; }
		
		
		public string Image { get; set; }
		
		
		public double Latitude { get; set; }
		
		
		public double Longitude { get; set; }
		
		
		public int SiteId { get; set; }
		
		
		public string SiteName { get; set; }
		
		

    }
}