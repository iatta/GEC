using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface ISystemConfigurationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSystemConfigurationForViewDto>> GetAll(GetAllSystemConfigurationsInput input);

        Task<GetSystemConfigurationForViewDto> GetSystemConfigurationForView(int id);

		Task<GetSystemConfigurationForEditOutput> GetSystemConfigurationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSystemConfigurationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSystemConfigurationsToExcel(GetAllSystemConfigurationsForExcelInput input);

		
    }
}