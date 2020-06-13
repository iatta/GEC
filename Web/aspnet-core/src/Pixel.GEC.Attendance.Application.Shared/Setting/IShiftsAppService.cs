using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.GEC.Attendance.Setting
{
    public interface IShiftsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftsInput input);

        Task<GetShiftForViewDto> GetShiftForView(int id);

		Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input);
        Task<List<GetShiftForViewDto>> GetAllFlat();



    }
}