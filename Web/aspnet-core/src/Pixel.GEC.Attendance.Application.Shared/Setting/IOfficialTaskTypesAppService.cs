using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
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