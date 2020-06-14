using Abp.AutoMapper;
using GEC.Attendance.Organizations.Dto;

namespace GEC.Attendance.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}