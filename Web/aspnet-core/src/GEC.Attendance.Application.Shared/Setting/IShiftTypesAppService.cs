using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
{
    public interface IShiftTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftTypeForViewDto>> GetAll(GetAllShiftTypesInput input);

        Task<GetShiftTypeForViewDto> GetShiftTypeForView(int id);

		Task<GetShiftTypeForEditOutput> GetShiftTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftTypesToExcel(GetAllShiftTypesForExcelInput input);

        Task<List<GetShiftTypeForViewDto>> GetAllFlat();

        



    }
}