using System.Collections.Generic;
using MvvmHelpers;
using Pixel.Attendance.Models.NavigationMenu;

namespace Pixel.Attendance.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}