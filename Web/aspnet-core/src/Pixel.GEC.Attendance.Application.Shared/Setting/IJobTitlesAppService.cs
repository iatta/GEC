using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
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