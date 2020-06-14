using Abp.Application.Services.Dto;

namespace GEC.Attendance.Operations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}