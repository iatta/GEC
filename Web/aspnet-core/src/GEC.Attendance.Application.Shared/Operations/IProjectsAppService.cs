using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Operations
{
    public interface IProjectsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input);

        Task<GetProjectForViewDto> GetProjectForView(int id);

		Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProjectDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input);

		
		Task<PagedResultDto<ProjectUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProjectLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}