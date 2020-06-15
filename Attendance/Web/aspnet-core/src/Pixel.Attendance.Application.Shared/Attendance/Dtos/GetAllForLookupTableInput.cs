using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Attendance.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}