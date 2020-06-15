using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}