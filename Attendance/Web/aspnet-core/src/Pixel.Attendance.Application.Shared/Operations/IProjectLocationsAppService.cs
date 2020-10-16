using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IProjectLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectLocationForViewDto>> GetAll(GetAllProjectLocationsInput input);

        Task<GetProjectLocationForViewDto> GetProjectLocationForView(int id);

		Task<GetProjectLocationForEditOutput> GetProjectLocationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProjectLocationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProjectLocationsToExcel(GetAllProjectLocationsForExcelInput input);

		
		Task<PagedResultDto<ProjectLocationProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProjectLocationLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}