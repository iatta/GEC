
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditEmployeeOfficialTaskDto : EntityDto<int?>
    {

		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		
		public string Remarks { get; set; }
		
		
		public string DescriptionAr { get; set; }
		
		
		public string DescriptionEn { get; set; }
		
		
		 public int? OfficialTaskTypeId { get; set; }

		public List<EmployeeOfficialTaskDetailDto> OfficialTaskDetails { get; set; }
		public List<long?> SelectedUsers { get; set; }


	}
}