
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operations.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {

		[Required]
		public string NameAr { get; set; }
		
		
		[Required]
		public string NameEn { get; set; }
		
		
		 public long? ManagerId { get; set; }
		 
		 		 public long? OrganizationUnitId { get; set; }
		 
		 		 public int? LocationId { get; set; }
		 
		 
    }
}