using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
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