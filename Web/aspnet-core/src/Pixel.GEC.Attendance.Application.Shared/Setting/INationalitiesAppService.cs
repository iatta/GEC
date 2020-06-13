using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
{
    public interface INationalitiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNationalityForViewDto>> GetAll(GetAllNationalitiesInput input);

		Task<GetNationalityForEditOutput> GetNationalityForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNationalityDto input);

		Task Delete(EntityDto input);

		
    }
}