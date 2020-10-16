
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class CreateOrEditLocationMachineDto : EntityDto<int?>
    {

		 public int? LocationId { get; set; }
		 
		 		 public int? MachineId { get; set; }
		 
		 
    }
}