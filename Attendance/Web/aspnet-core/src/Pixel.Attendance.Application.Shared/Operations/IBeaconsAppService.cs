using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IBeaconsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBeaconForViewDto>> GetAll(GetAllBeaconsInput input);

        Task<GetBeaconForViewDto> GetBeaconForView(int id);

		Task<GetBeaconForEditOutput> GetBeaconForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBeaconDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBeaconsToExcel(GetAllBeaconsForExcelInput input);

		
    }
}