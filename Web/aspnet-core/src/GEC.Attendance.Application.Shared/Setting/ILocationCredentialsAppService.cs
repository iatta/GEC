using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting
{
    public interface ILocationCredentialsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLocationCredentialForViewDto>> GetAll(GetAllLocationCredentialsInput input);

        Task<GetLocationCredentialForViewDto> GetLocationCredentialForView(int id);

		Task<GetLocationCredentialForEditOutput> GetLocationCredentialForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLocationCredentialDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLocationCredentialsToExcel(GetAllLocationCredentialsForExcelInput input);

		
		Task<PagedResultDto<LocationCredentialLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}