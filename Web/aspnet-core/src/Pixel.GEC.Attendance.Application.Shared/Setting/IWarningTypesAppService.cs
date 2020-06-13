using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
{
    public interface IWarningTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWarningTypeForViewDto>> GetAll(GetAllWarningTypesInput input);

        Task<GetWarningTypeForViewDto> GetWarningTypeForView(int id);

		Task<GetWarningTypeForEditOutput> GetWarningTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditWarningTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetWarningTypesToExcel(GetAllWarningTypesForExcelInput input);

		
    }
}