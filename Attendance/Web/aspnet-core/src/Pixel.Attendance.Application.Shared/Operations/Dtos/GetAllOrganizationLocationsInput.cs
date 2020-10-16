using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllOrganizationLocationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string LocationTitleEnFilter { get; set; }

		 
    }
}