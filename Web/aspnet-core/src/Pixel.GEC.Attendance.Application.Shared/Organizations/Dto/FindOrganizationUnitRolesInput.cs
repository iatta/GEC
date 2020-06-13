using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}