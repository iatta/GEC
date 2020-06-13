using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
{
    public interface ITempTransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTempTransactionForViewDto>> GetAll(GetAllTempTransactionsInput input);

        Task<GetTempTransactionForViewDto> GetTempTransactionForView(int id);

		Task<GetTempTransactionForEditOutput> GetTempTransactionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTempTransactionDto input);

		Task Delete(EntityDto input);

		
    }
}