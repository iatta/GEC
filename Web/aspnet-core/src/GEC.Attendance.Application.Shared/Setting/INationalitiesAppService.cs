using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting
{
    public interface INationalitiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNationalityForViewDto>> GetAll(GetAllNationalitiesInput input);

		Task<GetNationalityForEditOutput> GetNationalityForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNationalityDto input);

		Task Delete(EntityDto input);

		
    }
}