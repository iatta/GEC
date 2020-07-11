using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllBeaconsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string UidFilter { get; set; }

		public int? MaxMinorFilter { get; set; }
		public int? MinMinorFilter { get; set; }

		public int? MaxMajorFilter { get; set; }
		public int? MinMajorFilter { get; set; }



    }
}