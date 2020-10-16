
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class OrganizationLocationDto : EntityDto
    {

		 public long? OrganizationUnitId { get; set; }

		 		 public int? LocationId { get; set; }

        public string LocationName { get; set; }
    }
}