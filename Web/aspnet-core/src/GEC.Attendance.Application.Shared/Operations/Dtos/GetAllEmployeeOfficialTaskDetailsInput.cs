using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllEmployeeOfficialTaskDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string EmployeeOfficialTaskDescriptionArFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}