using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Settings.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Settings
{
    public interface IMobileWebPagesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMobileWebPageForViewDto>> GetAll(GetAllMobileWebPagesInput input);

        Task<GetMobileWebPageForViewDto> GetMobileWebPageForView(int id);
        Task<GetMobileWebPageForViewDto> GetMobileWebPageByName(string   name);

        Task<GetMobileWebPageForEditOutput> GetMobileWebPageForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditMobileWebPageDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetMobileWebPagesToExcel(GetAllMobileWebPagesForExcelInput input);

		
    }
}