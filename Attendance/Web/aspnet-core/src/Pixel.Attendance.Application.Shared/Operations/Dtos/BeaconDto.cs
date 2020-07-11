
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class BeaconDto : EntityDto
    {
		public string Name { get; set; }

		public string Uid { get; set; }

		public int Minor { get; set; }

		public int Major { get; set; }



    }
}