using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
{
    public interface IHolidaysAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHolidayForViewDto>> GetAll(GetAllHolidaysInput input);

        Task<GetHolidayForViewDto> GetHolidayForView(int id);

		Task<GetHolidayForEditOutput> GetHolidayForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditHolidayDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetHolidaysToExcel(GetAllHolidaysForExcelInput input);

		
    }
}