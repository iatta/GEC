using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using GEC.Attendance.Authorization.Users.Dto;
using GEC.Attendance.Security;
using GEC.Attendance.Web.Areas.App.Models.Common;

namespace GEC.Attendance.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
        public string MemberOrgenizationUnit { get; set; }
    }
}