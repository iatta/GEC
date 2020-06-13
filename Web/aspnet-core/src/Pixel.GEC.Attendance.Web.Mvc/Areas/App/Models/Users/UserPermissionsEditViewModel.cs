using Abp.AutoMapper;
using Pixel.GEC.Attendance.Authorization.Users;
using Pixel.GEC.Attendance.Authorization.Users.Dto;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}