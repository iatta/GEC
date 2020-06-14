using System.Globalization;

namespace GEC.Attendance.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}