using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using System.Collections.Generic;

namespace GEC.Attendance.Setting
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