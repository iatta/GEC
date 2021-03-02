using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Setting
{
    public interface ITaskTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTaskTypeForViewDto>> GetAll(GetAllTaskTypesInput input);

        Task<GetTaskTypeForViewDto> GetTaskTypeForView(int id);

		Task<GetTaskTypeForEditOutput> GetTaskTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTaskTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTaskTypesToExcel(GetAllTaskTypesForExcelInput input);
        Task<List<TaskTypeDto>> GetAllFlat();



    }
}