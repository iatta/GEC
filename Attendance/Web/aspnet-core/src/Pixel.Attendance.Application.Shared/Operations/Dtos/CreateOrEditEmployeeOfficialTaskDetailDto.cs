
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditEmployeeOfficialTaskDetailDto : EntityDto<int?>
    {

		 public int? EmployeeOfficialTaskId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}