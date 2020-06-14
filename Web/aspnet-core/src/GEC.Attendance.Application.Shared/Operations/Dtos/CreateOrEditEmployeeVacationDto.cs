
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Operations.Dtos
{
    public class CreateOrEditEmployeeVacationDto : EntityDto<int?>
    {

		public DateTime FromDate { get; set; }
		
		
		public DateTime ToDate { get; set; }
		
		
		public string Description { get; set; }
		
		
		public bool Status { get; set; }
		
		
		public string Note { get; set; }
		
		
		 public long UserId { get; set; }
		 
		public int LeaveTypeId { get; set; }
		public string LeaveCode { get; set; }
		public string EmpCode { get; set; }


	}
}