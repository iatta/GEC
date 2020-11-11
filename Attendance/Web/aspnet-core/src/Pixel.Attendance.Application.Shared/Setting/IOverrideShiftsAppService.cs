using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface IOverrideShiftsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetOverrideShiftForViewDto>> GetAll(GetAllOverrideShiftsInput input);

        Task<GetOverrideShiftForViewDto> GetOverrideShiftForView(int id);

		Task<GetOverrideShiftForEditOutput> GetOverrideShiftForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditOverrideShiftDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetOverrideShiftsToExcel(GetAllOverrideShiftsForExcelInput input);

		
		Task<PagedResultDto<OverrideShiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<OverrideShiftShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input);
		
    }
}