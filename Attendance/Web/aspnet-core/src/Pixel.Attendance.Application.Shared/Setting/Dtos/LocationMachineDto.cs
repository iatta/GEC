
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class LocationMachineDto : EntityDto
    {

		 public int? LocationId { get; set; }

		public int? MachineId { get; set; }
        public string MachineName { get; set; }


    }
}