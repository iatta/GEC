using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface IJobTitlesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobTitleForViewDto>> GetAll(GetAllJobTitlesInput input);

        Task<GetJobTitleForViewDto> GetJobTitleForView(int id);

		Task<GetJobTitleForEditOutput> GetJobTitleForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobTitleDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobTitlesToExcel(GetAllJobTitlesForExcelInput input);
        string GetTitleById(int id, int lang);


    }
}