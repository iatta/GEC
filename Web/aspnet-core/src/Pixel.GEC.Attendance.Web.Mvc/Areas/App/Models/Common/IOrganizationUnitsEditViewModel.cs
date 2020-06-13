using System.Collections.Generic;
using Pixel.GEC.Attendance.Organizations.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
        string MemberOrgenizationUnit { get; set; }
    }
}