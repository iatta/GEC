using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface IRamadanDatesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRamadanDateForViewDto>> GetAll(GetAllRamadanDatesInput input);

        Task<GetRamadanDateForViewDto> GetRamadanDateForView(int id);

		Task<GetRamadanDateForEditOutput> GetRamadanDateForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRamadanDateDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRamadanDatesToExcel(GetAllRamadanDatesForExcelInput input);

		
    }
}