using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting
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