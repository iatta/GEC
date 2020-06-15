using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface IPermitsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPermitForViewDto>> GetAll(GetAllPermitsInput input);

        Task<GetPermitForViewDto> GetPermitForView(int id);

		Task<GetPermitForEditOutput> GetPermitForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPermitDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPermitsToExcel(GetAllPermitsForExcelInput input);

		
    }
}