
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {

		public string NameAr { get; set; }
		
		
		public string NameEn { get; set; }

		public  string Code { get; set; }
		public string Number { get; set; }
		public long? ManagerId { get; set; }
		 
		public int? LocationId { get; set; }
		 
		public long? OrganizationUnitId { get; set; }
		 
		 
    }
}