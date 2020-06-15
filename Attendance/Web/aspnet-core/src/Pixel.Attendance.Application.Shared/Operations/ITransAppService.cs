using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface ITransAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTranForViewDto>> GetAll(GetAllTransInput input);

        Task<GetTranForViewDto> GetTranForView(int id);

		Task<GetTranForEditOutput> GetTranForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTranDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTransToExcel(GetAllTransForExcelInput input);

		
		Task<PagedResultDto<TranUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}