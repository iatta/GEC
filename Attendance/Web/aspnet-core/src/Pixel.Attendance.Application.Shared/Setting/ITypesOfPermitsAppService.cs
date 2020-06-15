using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface ITypesOfPermitsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTypesOfPermitForViewDto>> GetAll(GetAllTypesOfPermitsInput input);

        Task<GetTypesOfPermitForViewDto> GetTypesOfPermitForView(int id);

		Task<GetTypesOfPermitForEditOutput> GetTypesOfPermitForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTypesOfPermitDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTypesOfPermitsToExcel(GetAllTypesOfPermitsForExcelInput input);

		
    }
}