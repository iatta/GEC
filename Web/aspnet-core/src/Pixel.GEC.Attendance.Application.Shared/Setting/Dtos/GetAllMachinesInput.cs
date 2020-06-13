using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllMachinesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }

		public string NameEnFilter { get; set; }

		public string IpAddressFilter { get; set; }

		public string SubNetFilter { get; set; }

		public int StatusFilter { get; set; }


		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 
    }
}