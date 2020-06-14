using Abp.Application.Services.Dto;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}