using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.GEC.Attendance.Install.Dto;

namespace Pixel.GEC.Attendance.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}