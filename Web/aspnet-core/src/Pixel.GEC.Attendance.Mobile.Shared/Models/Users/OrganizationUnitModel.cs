using Abp.AutoMapper;
using Pixel.GEC.Attendance.Organizations.Dto;

namespace Pixel.GEC.Attendance.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}