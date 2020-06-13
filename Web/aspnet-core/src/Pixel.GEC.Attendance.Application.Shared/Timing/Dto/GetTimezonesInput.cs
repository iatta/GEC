using Abp.Configuration;

namespace Pixel.GEC.Attendance.Timing.Dto
{
    public class GetTimezonesInput
    {
        public SettingScopes DefaultTimezoneScope { get; set; }
    }
}
