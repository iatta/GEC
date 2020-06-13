using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization.Permissions.Dto;

namespace Pixel.GEC.Attendance.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
