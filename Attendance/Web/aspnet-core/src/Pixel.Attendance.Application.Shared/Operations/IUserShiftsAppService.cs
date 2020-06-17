using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
    public interface IUserShiftsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserShiftForViewDto>> GetAll(GetAllUserShiftsInput input);

        Task<GetUserShiftForViewDto> GetUserShiftForView(int id);

		Task<GetUserShiftForEditOutput> GetUserShiftForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUserShiftDto input);
		Task BulkCreateOrEdit(List<CreateOrEditUserShiftDto> input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUserShiftsToExcel(GetAllUserShiftsForExcelInput input);

		
		Task<PagedResultDto<UserShiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserShiftShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input);

		
    }
}