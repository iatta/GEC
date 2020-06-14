using Abp.Application.Services.Dto;

namespace GEC.Attendance.Settings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}