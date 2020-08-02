using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IUserDelegationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserDelegationForViewDto>> GetAll(GetAllUserDelegationsInput input);

        Task<GetUserDelegationForViewDto> GetUserDelegationForView(int id);

		Task<GetUserDelegationForEditOutput> GetUserDelegationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUserDelegationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUserDelegationsToExcel(GetAllUserDelegationsForExcelInput input);

		
		Task<PagedResultDto<UserDelegationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}