using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.GEC.Attendance.Operations
{
    public interface ITimeProfilesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTimeProfileForViewDto>> GetAll(GetAllTimeProfilesInput input);

        Task<GetTimeProfileForViewDto> GetTimeProfileForView(int id);

		Task<GetTimeProfileForEditOutput> GetTimeProfileForEdit(EntityDto input);

		Task CreateOrEdit(List<CreateOrEditTimeProfileDto> input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTimeProfilesToExcel(GetAllTimeProfilesForExcelInput input);

		
		Task<PagedResultDto<TimeProfileUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

		Task CreateTimeProfileFromExcel(List<CreateTimeProfileFromExcelDto> input);


	}
}