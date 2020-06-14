using Abp.Application.Services.Dto;
using System;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllTempTransactionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}