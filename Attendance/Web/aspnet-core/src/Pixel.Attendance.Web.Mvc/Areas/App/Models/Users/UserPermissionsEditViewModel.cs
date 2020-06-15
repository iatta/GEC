using Abp.AutoMapper;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.Attendance.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}