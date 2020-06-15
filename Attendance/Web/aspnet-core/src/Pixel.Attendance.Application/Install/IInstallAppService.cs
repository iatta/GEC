using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.Install.Dto;

namespace Pixel.Attendance.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}