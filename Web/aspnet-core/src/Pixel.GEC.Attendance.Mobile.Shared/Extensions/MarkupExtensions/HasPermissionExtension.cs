using System;
using Pixel.GEC.Attendance.Core;
using Pixel.GEC.Attendance.Core.Dependency;
using Pixel.GEC.Attendance.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pixel.GEC.Attendance.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}