using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Organizations.Dto
{
    public class GetOrganizationUnitRolesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "role.DisplayName, role.Name";
            }
            else if (Sorting.Contains("displayName"))
            {
                Sorting = Sorting.Replace("displayName", "role.displayName");
            }
            else if (Sorting.Contains("addedTime"))
            {
                Sorting = Sorting.Replace("addedTime", "uou.creationTime");
            }
        }
    }
}