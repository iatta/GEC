
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditOrganizationLocationDto : EntityDto<int?>
    {

		 public long? OrganizationUnitId { get; set; }
		 
		 		 public int? LocationId { get; set; }
		 
		 
    }
}