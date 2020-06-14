
using System;
using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operations.Dtos
{
    public class ProjectDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }


		 public long? ManagerId { get; set; }

		 		 public long? OrganizationUnitId { get; set; }

		 		 public int? LocationId { get; set; }

		 
    }
}