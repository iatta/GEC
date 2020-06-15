using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}