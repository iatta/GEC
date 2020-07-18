
using System;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class ProjectDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }
		public string Code { get; set; }
		public string Number { get; set; }


		public long? ManagerId { get; set; }

		 		 public int? LocationId { get; set; }

		 		 public long? OrganizationUnitId { get; set; }

		 
    }
}