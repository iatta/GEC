using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetLocationMachineForEditOutput
    {
		public CreateOrEditLocationMachineDto LocationMachine { get; set; }

		public string LocationTitleAr { get; set;}

		public string MachineNameEn { get; set;}


    }
}