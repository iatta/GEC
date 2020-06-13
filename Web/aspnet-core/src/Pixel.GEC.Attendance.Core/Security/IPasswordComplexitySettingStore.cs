using System.Threading.Tasks;

namespace Pixel.GEC.Attendance.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
