
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class ProjectLocationDto : EntityDto
    {

		 public int? ProjectId { get; set; }

		public int? LocationId { get; set; }
        public string LocationName { get; set; }


    }
}