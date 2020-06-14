using System.Collections.Generic;
using MvvmHelpers;
using GEC.Attendance.Models.NavigationMenu;

namespace GEC.Attendance.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}