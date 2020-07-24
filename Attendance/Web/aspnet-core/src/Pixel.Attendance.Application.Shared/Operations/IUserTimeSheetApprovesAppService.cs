using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IUserTimeSheetApprovesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserTimeSheetApproveForViewDto>> GetAll(GetAllUserTimeSheetApprovesInput input);

        Task<GetUserTimeSheetApproveForViewDto> GetUserTimeSheetApproveForView(int id);

		Task<GetUserTimeSheetApproveForEditOutput> GetUserTimeSheetApproveForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUserTimeSheetApproveDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUserTimeSheetApprovesToExcel(GetAllUserTimeSheetApprovesForExcelInput input);

		
		Task<PagedResultDto<UserTimeSheetApproveUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}