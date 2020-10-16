using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface ILocationMachinesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLocationMachineForViewDto>> GetAll(GetAllLocationMachinesInput input);

        Task<GetLocationMachineForViewDto> GetLocationMachineForView(int id);

		Task<GetLocationMachineForEditOutput> GetLocationMachineForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLocationMachineDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLocationMachinesToExcel(GetAllLocationMachinesForExcelInput input);

		
		Task<PagedResultDto<LocationMachineLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<LocationMachineMachineLookupTableDto>> GetAllMachineForLookupTable(GetAllForLookupTableInput input);
		
    }
}