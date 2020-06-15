using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Settings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}