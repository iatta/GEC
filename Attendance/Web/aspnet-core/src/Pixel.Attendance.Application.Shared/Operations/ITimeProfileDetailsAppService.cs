using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface ITimeProfileDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTimeProfileDetailForViewDto>> GetAll(GetAllTimeProfileDetailsInput input);

        Task<GetTimeProfileDetailForViewDto> GetTimeProfileDetailForView(int id);

		Task<GetTimeProfileDetailForEditOutput> GetTimeProfileDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTimeProfileDetailDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTimeProfileDetailsToExcel(GetAllTimeProfileDetailsForExcelInput input);

		
		Task<PagedResultDto<TimeProfileDetailTimeProfileLookupTableDto>> GetAllTimeProfileForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TimeProfileDetailShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input);
		
    }
}