using Pixel.Attendance.Operations;
using Pixel.Attendance.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.Attendance.Operations.Exporting;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.Attendance.Operations
{
	//[AbpAuthorize(AppPermissions.Pages_TransactionLogs)]
    public class TransactionLogsAppService : AttendanceAppServiceBase, ITransactionLogsAppService
    {
		 private readonly IRepository<TransactionLog> _transactionLogRepository;
		 private readonly ITransactionLogsExcelExporter _transactionLogsExcelExporter;
		 private readonly IRepository<Transaction,int> _lookup_transactionRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public TransactionLogsAppService(IRepository<TransactionLog> transactionLogRepository, ITransactionLogsExcelExporter transactionLogsExcelExporter , IRepository<Transaction, int> lookup_transactionRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_transactionLogRepository = transactionLogRepository;
			_transactionLogsExcelExporter = transactionLogsExcelExporter;
			_lookup_transactionRepository = lookup_transactionRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

        public async Task<List<GetTransactionLogForViewDto>> GetTransactionLogByTransId(int inId , int outId)
        {
            var output = new List<GetTransactionLogForViewDto>();

            var transactionLogs = _transactionLogRepository.GetAll().Where(x => x.TransactionId == inId  || x.TransactionId == outId).ToList();
            foreach (var transactionLog in transactionLogs)
            {
                var transactionToAdd = new GetTransactionLogForViewDto { TransactionLog = ObjectMapper.Map<TransactionLogDto>(transactionLog) };
                if (transactionToAdd.TransactionLog.TransactionId != null)
                {
                    var _lookupTransaction = await _lookup_transactionRepository.FirstOrDefaultAsync((int)transactionToAdd.TransactionLog.TransactionId.Value);
                    transactionToAdd.TransactionTransaction_Date = _lookupTransaction.Transaction_Date.ToString();
                }

                if (transactionToAdd.TransactionLog.ActionBy != null)
                {
                    var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)transactionToAdd.TransactionLog.ActionBy);
                    transactionToAdd.UserName = _lookupUser.Name.ToString();
                }
                if (transactionToAdd.TransactionLog.NewValue == transactionToAdd.TransactionLog.OldValue)
                {
                    transactionToAdd.TransactionLog.HasDifferent = true;
                }
                output.Add(transactionToAdd);
            }

            return output;

            
        }

		 public async Task<PagedResultDto<GetTransactionLogForViewDto>> GetAll(GetAllTransactionLogsInput input)
         {
			
			var filteredTransactionLogs = _transactionLogRepository.GetAll()
						.Include( e => e.TransactionFk)
						.Include( e => e.ActionFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.OldValue.Contains(input.Filter) || e.NewValue.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.OldValueFilter),  e => e.OldValue == input.OldValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NewValueFilter),  e => e.NewValue == input.NewValueFilter)
						
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ActionFk != null && e.ActionFk.Name == input.UserNameFilter);

			var pagedAndFilteredTransactionLogs = filteredTransactionLogs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var transactionLogs = from o in pagedAndFilteredTransactionLogs
                         join o1 in _lookup_transactionRepository.GetAll() on o.TransactionId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.ActionBy equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTransactionLogForViewDto() {
							TransactionLog = new TransactionLogDto
							{
                                OldValue = o.OldValue,
                                NewValue = o.NewValue,
                                Id = o.Id
							},
                         	TransactionTransaction_Date = s1 == null ? "" : s1.Transaction_Date.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredTransactionLogs.CountAsync();

            return new PagedResultDto<GetTransactionLogForViewDto>(
                totalCount,
                await transactionLogs.ToListAsync()
            );
         }
		 
		 public async Task<GetTransactionLogForViewDto> GetTransactionLogForView(int id)
         {
            var transactionLog = await _transactionLogRepository.GetAsync(id);

            var output = new GetTransactionLogForViewDto { TransactionLog = ObjectMapper.Map<TransactionLogDto>(transactionLog) };

		    if (output.TransactionLog.TransactionId != null)
            {
                var _lookupTransaction = await _lookup_transactionRepository.FirstOrDefaultAsync((int)output.TransactionLog.TransactionId);
                output.TransactionTransaction_Date = _lookupTransaction.Transaction_Date.ToString();
            }

		    if (output.TransactionLog.ActionBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TransactionLog.ActionBy);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TransactionLogs_Edit)]
		 public async Task<GetTransactionLogForEditOutput> GetTransactionLogForEdit(EntityDto input)
         {
            var transactionLog = await _transactionLogRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTransactionLogForEditOutput {TransactionLog = ObjectMapper.Map<CreateOrEditTransactionLogDto>(transactionLog)};

		    if (output.TransactionLog.TransactionId != null)
            {
                var _lookupTransaction = await _lookup_transactionRepository.FirstOrDefaultAsync((int)output.TransactionLog.TransactionId);
                output.TransactionTransaction_Date = _lookupTransaction.Transaction_Date.ToString();
            }

		    if (output.TransactionLog.ActionBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TransactionLog.ActionBy);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTransactionLogDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TransactionLogs_Create)]
		 protected virtual async Task Create(CreateOrEditTransactionLogDto input)
         {
            var transactionLog = ObjectMapper.Map<TransactionLog>(input);

			

            await _transactionLogRepository.InsertAsync(transactionLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_TransactionLogs_Edit)]
		 protected virtual async Task Update(CreateOrEditTransactionLogDto input)
         {
            var transactionLog = await _transactionLogRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, transactionLog);
         }

		 [AbpAuthorize(AppPermissions.Pages_TransactionLogs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _transactionLogRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTransactionLogsToExcel(GetAllTransactionLogsForExcelInput input)
         {
			
			var filteredTransactionLogs = _transactionLogRepository.GetAll()
						.Include( e => e.TransactionFk)
						.Include( e => e.ActionFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.OldValue.Contains(input.Filter) || e.NewValue.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.OldValueFilter),  e => e.OldValue == input.OldValueFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NewValueFilter),  e => e.NewValue == input.NewValueFilter)
						
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ActionFk != null && e.ActionFk.Name == input.UserNameFilter);

			var query = (from o in filteredTransactionLogs
                         join o1 in _lookup_transactionRepository.GetAll() on o.TransactionId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.ActionBy equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTransactionLogForViewDto() { 
							TransactionLog = new TransactionLogDto
							{
                                OldValue = o.OldValue,
                                NewValue = o.NewValue,
                                Id = o.Id
							},
                         	TransactionTransaction_Date = s1 == null ? "" : s1.Transaction_Date.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						 });


            var transactionLogListDtos = await query.ToListAsync();

            return _transactionLogsExcelExporter.ExportToFile(transactionLogListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TransactionLogs)]
         public async Task<PagedResultDto<TransactionLogTransactionLookupTableDto>> GetAllTransactionForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_transactionRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Transaction_Date.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var transactionList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TransactionLogTransactionLookupTableDto>();
			foreach(var transaction in transactionList){
				lookupTableDtoList.Add(new TransactionLogTransactionLookupTableDto
				{
					Id = transaction.Id,
					
				});
			}

            return new PagedResultDto<TransactionLogTransactionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TransactionLogs)]
         public async Task<PagedResultDto<TransactionLogUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TransactionLogUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new TransactionLogUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<TransactionLogUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}