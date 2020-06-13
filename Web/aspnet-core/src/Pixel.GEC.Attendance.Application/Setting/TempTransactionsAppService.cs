

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.Setting
{
	[AbpAuthorize(AppPermissions.Pages_TempTransactions)]
    public class TempTransactionsAppService : AttendanceAppServiceBase, ITempTransactionsAppService
    {
		 private readonly IRepository<TempTransaction> _tempTransactionRepository;
		 

		  public TempTransactionsAppService(IRepository<TempTransaction> tempTransactionRepository ) 
		  {
			_tempTransactionRepository = tempTransactionRepository;
			
		  }

		 public async Task<PagedResultDto<GetTempTransactionForViewDto>> GetAll(GetAllTempTransactionsInput input)
         {
			
			var filteredTempTransactions = _tempTransactionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.In.Contains(input.Filter) || e.Out.Contains(input.Filter) || e.LI.Contains(input.Filter) || e.EO.Contains(input.Filter) || e.Permission_Text.Contains(input.Filter) || e.OfficialTask_Text.Contains(input.Filter));

			var pagedAndFilteredTempTransactions = filteredTempTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var tempTransactions = from o in pagedAndFilteredTempTransactions
                         select new GetTempTransactionForViewDto() {
							TempTransaction = new TempTransactionDto
							{
                                Id = o.Id
							}
						};

            var totalCount = await filteredTempTransactions.CountAsync();

            return new PagedResultDto<GetTempTransactionForViewDto>(
                totalCount,
                await tempTransactions.ToListAsync()
            );
         }
		 
		 public async Task<GetTempTransactionForViewDto> GetTempTransactionForView(int id)
         {
            var tempTransaction = await _tempTransactionRepository.GetAsync(id);

            var output = new GetTempTransactionForViewDto { TempTransaction = ObjectMapper.Map<TempTransactionDto>(tempTransaction) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TempTransactions_Edit)]
		 public async Task<GetTempTransactionForEditOutput> GetTempTransactionForEdit(EntityDto input)
         {
            var tempTransaction = await _tempTransactionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTempTransactionForEditOutput {TempTransaction = ObjectMapper.Map<CreateOrEditTempTransactionDto>(tempTransaction)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTempTransactionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TempTransactions_Create)]
		 protected virtual async Task Create(CreateOrEditTempTransactionDto input)
         {
            var tempTransaction = ObjectMapper.Map<TempTransaction>(input);

			

            await _tempTransactionRepository.InsertAsync(tempTransaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_TempTransactions_Edit)]
		 protected virtual async Task Update(CreateOrEditTempTransactionDto input)
         {
            var tempTransaction = await _tempTransactionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, tempTransaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_TempTransactions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _tempTransactionRepository.DeleteAsync(input.Id);
         } 
    }
}