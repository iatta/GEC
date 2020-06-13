using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}