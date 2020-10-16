using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IOrganizationLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetOrganizationLocationForViewDto>> GetAll(GetAllOrganizationLocationsInput input);

        Task<GetOrganizationLocationForViewDto> GetOrganizationLocationForView(int id);

		Task<GetOrganizationLocationForEditOutput> GetOrganizationLocationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditOrganizationLocationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetOrganizationLocationsToExcel(GetAllOrganizationLocationsForExcelInput input);

		
		Task<PagedResultDto<OrganizationLocationOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<OrganizationLocationLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}