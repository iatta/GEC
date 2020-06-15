using System.Threading.Tasks;

namespace Pixel.Attendance.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
