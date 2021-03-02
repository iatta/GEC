
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {
        public CreateOrEditProjectDto()
        {
			Locations = new List<ProjectLocationDto>();

		}
		public string NameAr { get; set; }
		
		
		public string NameEn { get; set; }

		public  string Code { get; set; }
		public string Number { get; set; }
		public long? ManagerId { get; set; }
		 
		public int? LocationId { get; set; }
		public long? ManagerAssistantId { get; set; }
		public long? OrganizationUnitId { get; set; }
        public ICollection<ProjectLocationDto> Locations { get; set; }


    }
}