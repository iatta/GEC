using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Authorization.Permissions.Dto;

namespace Pixel.Attendance.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
