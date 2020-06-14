using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting
{
    public interface IShiftTypeDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftTypeDetailForViewDto>> GetAll(GetAllShiftTypeDetailsInput input);

        Task<GetShiftTypeDetailForViewDto> GetShiftTypeDetailForView(int id);

		Task<GetShiftTypeDetailForEditOutput> GetShiftTypeDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftTypeDetailDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftTypeDetailsToExcel(GetAllShiftTypeDetailsForExcelInput input);

		
		Task<PagedResultDto<ShiftTypeDetailShiftTypeLookupTableDto>> GetAllShiftTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}