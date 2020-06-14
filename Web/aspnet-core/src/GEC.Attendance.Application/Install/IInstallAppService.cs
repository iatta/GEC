using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.Install.Dto;

namespace GEC.Attendance.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}