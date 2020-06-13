using System.Globalization;

namespace Pixel.GEC.Attendance.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}