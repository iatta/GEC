

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using GEC.Attendance.Operations.Exporting;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using GEC.Attendance.Authorization.Users;

namespace GEC.Attendance.Operations
{
    [AbpAuthorize(AppPermissions.Pages_ManualTransactions)]
    public class TransactionsAppService : AttendanceAppServiceBase, ITransactionsAppService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly ITransactionsExcelExporter _transactionsExcelExporter;
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _lookup_userRepository;


        public TransactionsAppService(IRepository<Transaction> transactionRepository, ITransactionsExcelExporter transactionsExcelExporter, UserManager userManager, IRepository<User, long> lookup_userRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionsExcelExporter = transactionsExcelExporter;
            _userManager = userManager;
            _lookup_userRepository = lookup_userRepository;


        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input)
        {

            var filteredTransactions = _transactionRepository.GetAll()
                        .Where(x => x.Manual == 1)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Time.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Remark.Contains(input.Filter))
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter);
                        

            var pagedAndFilteredTransactions = filteredTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var transactions = from o in pagedAndFilteredTransactions
                               join o1 in _lookup_userRepository.GetAll() on o.Pin equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()
                               select new GetTransactionForViewDto()
                                {
                                    Transaction = new TransactionDto
                                    {
                                        TransType = o.TransType,
                                        KeyType = o.KeyType,
                                        CreationTime = o.CreationTime,
                                        Transaction_Date = o.Transaction_Date,
                                        Pin = o.Pin,
                                        Time = o.Time,
                                        Id = o.Id
                                    },
                                    UserName = s1 == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredTransactions.CountAsync();

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
                await transactions.ToListAsync()
            );
        }

        public async Task<GetTransactionForViewDto> GetTransactionForView(int id)
        {
            var transaction = await _transactionRepository.GetAsync(id);

            var output = new GetTransactionForViewDto { Transaction = ObjectMapper.Map<TransactionDto>(transaction) };
            if (output.Transaction.Pin > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Transaction.Pin);
                output.UserName = _lookupUser.Name.ToString();
            }


            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
        public async Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto input)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionForEditOutput { Transaction = ObjectMapper.Map<CreateOrEditTransactionDto>(transaction) };

            if (output.Transaction.Pin  > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Transaction.Pin);
                output.UserName = _lookupUser.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTransactionDto input)
        {
            
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Create)]
        protected virtual async Task Create(CreateOrEditTransactionDto input)
        {
            input.Manual = 1;
            input.ImportDate = new DateTime();

            var transaction = ObjectMapper.Map<Transaction>(input);
            await _transactionRepository.InsertAsync(transaction);
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
        protected virtual async Task Update(CreateOrEditTransactionDto input)
        {
            input.Manual = 1;
            input.ImportDate = new DateTime();


            var transaction = await _transactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transaction);
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _transactionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTransactionsToExcel(GetAllTransactionsForExcelInput input)
        {

            var filteredTransactions = _transactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Time.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Remark.Contains(input.Filter))
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter);

            var query = (from o in filteredTransactions
                         select new GetTransactionForViewDto()
                         {
                             Transaction = new TransactionDto
                             {
                                 TransType = o.TransType,
                                 Id = o.Id
                             }
                         });


            var transactionListDtos = await query.ToListAsync();

            return _transactionsExcelExporter.ExportToFile(transactionListDtos);
        }


    }
}