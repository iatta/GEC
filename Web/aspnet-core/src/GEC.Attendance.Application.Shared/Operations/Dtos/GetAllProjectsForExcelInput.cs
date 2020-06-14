using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllProjectsForExcelInput
    {
		public string Filter { get; set; }

		public string NameArFilter { get; set; }

		public string NameEnFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string LocationTitleEnFilter { get; set; }

		 
    }
}