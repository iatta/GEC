using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Setting
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