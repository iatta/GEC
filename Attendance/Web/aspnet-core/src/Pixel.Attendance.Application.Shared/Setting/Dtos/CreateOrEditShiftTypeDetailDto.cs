
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditShiftTypeDetailDto : EntityDto<int?>
    {

		public bool InTimeFirstScan { get; set; }
		
		
		public bool InTimeLastScan { get; set; }
		
		
		public bool OutTimeFirstScan { get; set; }
		
		
		public bool OutTimeLastScan { get; set; }
		
		
		public int NoDuty { get; set; }
		
		
		 public int ShiftTypeId { get; set; }
		 
		 
    }
}