using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllTypesOfPermitsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}