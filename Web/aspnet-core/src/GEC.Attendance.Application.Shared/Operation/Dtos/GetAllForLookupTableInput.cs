using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operation.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}