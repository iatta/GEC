using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting
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
		
    }
}