using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Operation.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}