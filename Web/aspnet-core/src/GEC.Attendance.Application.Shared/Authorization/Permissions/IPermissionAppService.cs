using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization.Permissions.Dto;

namespace GEC.Attendance.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
