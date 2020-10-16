
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditProjectLocationDto : EntityDto<int?>
    {

		 public int? ProjectId { get; set; }
		 
		 		 public int? LocationId { get; set; }
		 
		 
    }
}