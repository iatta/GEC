
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditBeaconDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Uid { get; set; }
		
		
		public int Minor { get; set; }
		
		
		public int Major { get; set; }
		
		

    }
}