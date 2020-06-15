using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}