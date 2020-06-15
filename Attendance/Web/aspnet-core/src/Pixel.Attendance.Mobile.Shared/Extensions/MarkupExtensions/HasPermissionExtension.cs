using System;
using Pixel.Attendance.Core;
using Pixel.Attendance.Core.Dependency;
using Pixel.Attendance.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pixel.Attendance.Extensions.MarkupExtensions
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