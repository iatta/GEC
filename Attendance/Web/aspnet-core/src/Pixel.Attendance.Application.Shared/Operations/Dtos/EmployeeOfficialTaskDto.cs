
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class EmployeeOfficialTaskDto : EntityDto
    {
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string Remarks { get; set; }

		public string DescriptionAr { get; set; }

		public string DescriptionEn { get; set; }


		 public int? OfficialTaskTypeId { get; set; }

		public List<EmployeeOfficialTaskDetailDto> OfficialTaskDetails { get; set; }


	}
}