using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operation.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}