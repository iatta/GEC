using Abp.Application.Services.Dto;
using System;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllOfficialTaskTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }

		public string NameEnFilter { get; set; }

		public int TypeInFilter { get; set; }

		public int TypeOutFilter { get; set; }

		public int TypeInOutFilter { get; set; }



    }
}