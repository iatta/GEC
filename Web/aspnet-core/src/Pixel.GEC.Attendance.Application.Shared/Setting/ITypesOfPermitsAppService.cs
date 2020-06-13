using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
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