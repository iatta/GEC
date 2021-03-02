using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Setting
{
    public interface IMachinesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMachineForViewDto>> GetAll(GetAllMachinesInput input);

        Task<GetMachineForViewDto> GetMachineForView(int id);

		Task<GetMachineForEditOutput> GetMachineForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditMachineDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetMachinesToExcel(GetAllMachinesForExcelInput input);

		
		Task<PagedResultDto<MachineOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		Task<PagedResultDto<MachineLookupTableDto>> GetAllMachinesForLookupTable(GetAllForLookupTableInput input);
		Task<List<MachineDto>> GetAllFlat();

	}
}