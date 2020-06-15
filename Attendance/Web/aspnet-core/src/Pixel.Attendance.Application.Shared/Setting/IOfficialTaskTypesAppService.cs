using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface IOfficialTaskTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetOfficialTaskTypeForViewDto>> GetAll(GetAllOfficialTaskTypesInput input);

        Task<GetOfficialTaskTypeForViewDto> GetOfficialTaskTypeForView(int id);

		Task<GetOfficialTaskTypeForEditOutput> GetOfficialTaskTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditOfficialTaskTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetOfficialTaskTypesToExcel(GetAllOfficialTaskTypesForExcelInput input);

		
    }
}