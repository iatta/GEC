using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllLocationMachinesForExcelInput
    {
		public string Filter { get; set; }


		 public string LocationTitleArFilter { get; set; }

		 		 public string MachineNameEnFilter { get; set; }

		 
    }
}