using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}