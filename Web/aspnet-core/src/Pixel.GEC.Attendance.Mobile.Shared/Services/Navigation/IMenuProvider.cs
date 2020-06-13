using System.Collections.Generic;
using MvvmHelpers;
using Pixel.GEC.Attendance.Models.NavigationMenu;

namespace Pixel.GEC.Attendance.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}