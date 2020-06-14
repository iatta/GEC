using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Organizations.Dto
{
    public class UsersToOrganizationUnitInput
    {
        public long[] UserIds { get; set; }

        [Range(1, long.MaxValue)]
        public int OrganizationUnitId { get; set; }
    }
}