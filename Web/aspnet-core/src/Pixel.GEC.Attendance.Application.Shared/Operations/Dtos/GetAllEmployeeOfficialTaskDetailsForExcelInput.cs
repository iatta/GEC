﻿using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetAllEmployeeOfficialTaskDetailsForExcelInput
    {
		public string Filter { get; set; }


		 public string EmployeeOfficialTaskDescriptionArFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}