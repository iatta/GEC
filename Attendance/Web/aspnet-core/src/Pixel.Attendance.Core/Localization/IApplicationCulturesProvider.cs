using System.Globalization;

namespace Pixel.Attendance.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}