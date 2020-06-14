using GEC.Attendance.Dto;

namespace GEC.Attendance.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}