using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetBeaconForEditOutput
    {
		public CreateOrEditBeaconDto Beacon { get; set; }


    }
}