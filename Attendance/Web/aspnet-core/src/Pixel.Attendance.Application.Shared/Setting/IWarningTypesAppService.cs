using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
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