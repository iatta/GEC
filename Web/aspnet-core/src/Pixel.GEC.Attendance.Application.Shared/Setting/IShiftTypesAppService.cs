using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.GEC.Attendance.Setting
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