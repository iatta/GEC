using System;
using Pixel.GEC.Attendance.Core;
using Pixel.GEC.Attendance.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pixel.GEC.Attendance.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return Text;
            }

            return L.Localize(Text);
        }
    }
}