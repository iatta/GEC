using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllProjectLocationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ProjectNameEnFilter { get; set; }

		 		 public string LocationTitleEnFilter { get; set; }

		 
    }
}