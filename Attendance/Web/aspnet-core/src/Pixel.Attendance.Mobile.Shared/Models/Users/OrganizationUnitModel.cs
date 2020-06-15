using Abp.AutoMapper;
using Pixel.Attendance.Organizations.Dto;

namespace Pixel.Attendance.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}