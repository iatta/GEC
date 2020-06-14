using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllShiftTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionEnFilter { get; set; }

		public string DescriptionArFilter { get; set; }



    }
}