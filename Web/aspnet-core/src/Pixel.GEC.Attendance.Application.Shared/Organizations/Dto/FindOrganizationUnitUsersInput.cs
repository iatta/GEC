using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
