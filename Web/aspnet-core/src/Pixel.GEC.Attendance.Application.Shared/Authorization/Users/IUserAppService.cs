using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization.Users.Dto;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);
        Task<GetUserForEditOutput> GetExistUSerForActive(string civilId);

        Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input);

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        Task ResetUserSpecificPermissions(EntityDto<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(EntityDto<long> input);

        Task UnlockUser(EntityDto<long> input);
        Task<List<UserListDto>> GetUsersFlat();
        Task<List<UserListDto>> GetUsersFlatByUnitId(int unitId);

        Task<GetUserForFaceIdOutput> GetUserForFaceId(string civilId);
        Task UpdateUserFaceId(UpdateUserFaceIdInput input);
        Task<List<UserReportDto>> GetUsersByShiftId(int shiftId);

        Task<List<InOutReportOutput>> GenerateInOutReport(ReportInput input);
        Task<List<FingerReportOutput>> GenerateFingerReport(ReportInput input);
        Task<List<InOutReportOutput>> GenerateForgetInOutReport(ReportInput input);
        Task<List<PermitReportOutput>> GeneratePermitReport(ReportInput input);
        Task<List<EmployeeReportOutput>> CalculateDaysReport(ReportInput input);
    }
}