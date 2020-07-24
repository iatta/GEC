using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;


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
	[AbpAuthorize(AppPermissions.Pages_ManualTransactions)]
    public class ManualTransactionsAppService : AttendanceAppServiceBase, IManualTransactionsAppService
    {
		 private readonly IRepository<ManualTransaction> _manualTransactionRepository;
		 private readonly IManualTransactionsExcelExporter _manualTransactionsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Machine,int> _lookup_machineRepository;
        private readonly UserManager _userManager;


        public ManualTransactionsAppService(IRepository<ManualTransaction> manualTransactionRepository, IManualTransactionsExcelExporter manualTransactionsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Machine, int> lookup_machineRepository, UserManager userManager) 
		  {
			_manualTransactionRepository = manualTransactionRepository;
			_manualTransactionsExcelExporter = manualTransactionsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		    _lookup_machineRepository = lookup_machineRepository;
            _userManager = userManager;

        }

		 public async Task<PagedResultDto<GetManualTransactionForViewDto>> GetAll(GetAllManualTransactionsInput input)
         {
			
			var filteredManualTransactions = _manualTransactionRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.MachineFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FingerCode.Contains(input.Filter) || e.CivilId.Contains(input.Filter) || e.Image.Contains(input.Filter))
						.WhereIf(input.MinTransDateFilter != null, e => e.TransDate >= input.MinTransDateFilter)
						.WhereIf(input.MaxTransDateFilter != null, e => e.TransDate <= input.MaxTransDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MachineNameEnFilter), e => e.MachineFk != null && e.MachineFk.NameEn == input.MachineNameEnFilter);

			var pagedAndFilteredManualTransactions = filteredManualTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var manualTransactions = from o in pagedAndFilteredManualTransactions
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_machineRepository.GetAll() on o.MachineId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetManualTransactionForViewDto() {
							ManualTransaction = new ManualTransactionDto
							{
                                TransDate = o.TransDate,
                                TransType = o.TransType,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	MachineNameEn = s2 == null ? "" : s2.NameEn.ToString()
						};

            var totalCount = await filteredManualTransactions.CountAsync();

            return new PagedResultDto<GetManualTransactionForViewDto>(
                totalCount,
                await manualTransactions.ToListAsync()
            );
         }
		 
		 public async Task<GetManualTransactionForViewDto> GetManualTransactionForView(int id)
         {
            var manualTransaction = await _manualTransactionRepository.GetAsync(id);

            var output = new GetManualTransactionForViewDto { ManualTransaction = ObjectMapper.Map<ManualTransactionDto>(manualTransaction) };

		    if (output.ManualTransaction.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ManualTransaction.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ManualTransaction.MachineId != null)
            {
                var _lookupMachine = await _lookup_machineRepository.FirstOrDefaultAsync((int)output.ManualTransaction.MachineId);
                output.MachineNameEn = _lookupMachine.NameEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
		 public async Task<GetManualTransactionForEditOutput> GetManualTransactionForEdit(EntityDto input)
         {
            var manualTransaction = await _manualTransactionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetManualTransactionForEditOutput {ManualTransaction = ObjectMapper.Map<CreateOrEditManualTransactionDto>(manualTransaction)};

		    if (output.ManualTransaction.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ManualTransaction.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ManualTransaction.MachineId != null)
            {
                var _lookupMachine = await _lookup_machineRepository.FirstOrDefaultAsync((int)output.ManualTransaction.MachineId);
                output.MachineNameEn = _lookupMachine.NameEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditManualTransactionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Create)]
		 protected virtual async Task Create(CreateOrEditManualTransactionDto input)
         {
            var user = await _userManager.FindByIdAsync(input.UserId.ToString());

            var manualTransaction = ObjectMapper.Map<ManualTransaction>(input);
            manualTransaction.FingerCode = user.FingerCode;
            manualTransaction.CivilId = user.CivilId;
            await _manualTransactionRepository.InsertAsync(manualTransaction);
        }

		 [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
		 protected virtual async Task Update(CreateOrEditManualTransactionDto input)
         {
            var user = await _userManager.FindByIdAsync(input.UserId.ToString());

            var manualTransaction = await _manualTransactionRepository.FirstOrDefaultAsync((int)input.Id);
            manualTransaction.FingerCode = user.FingerCode;
            manualTransaction.CivilId = user.CivilId;

            ObjectMapper.Map(input, manualTransaction);
        }

		 [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _manualTransactionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetManualTransactionsToExcel(GetAllManualTransactionsForExcelInput input)
         {
			
			var filteredManualTransactions = _manualTransactionRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.MachineFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FingerCode.Contains(input.Filter) || e.CivilId.Contains(input.Filter) || e.Image.Contains(input.Filter))
						.WhereIf(input.MinTransDateFilter != null, e => e.TransDate >= input.MinTransDateFilter)
						.WhereIf(input.MaxTransDateFilter != null, e => e.TransDate <= input.MaxTransDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MachineNameEnFilter), e => e.MachineFk != null && e.MachineFk.NameEn == input.MachineNameEnFilter);

			var query = (from o in filteredManualTransactions
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_machineRepository.GetAll() on o.MachineId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetManualTransactionForViewDto() { 
							ManualTransaction = new ManualTransactionDto
							{
                                TransDate = o.TransDate,
                                TransType = o.TransType,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	MachineNameEn = s2 == null ? "" : s2.NameEn.ToString()
						 });


            var manualTransactionListDtos = await query.ToListAsync();

            return _manualTransactionsExcelExporter.ExportToFile(manualTransactionListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ManualTransactions)]
         public async Task<PagedResultDto<ManualTransactionUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ManualTransactionUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ManualTransactionUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ManualTransactionUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ManualTransactions)]
         public async Task<PagedResultDto<ManualTransactionMachineLookupTableDto>> GetAllMachineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_machineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var machineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ManualTransactionMachineLookupTableDto>();
			foreach(var machine in machineList){
				lookupTableDtoList.Add(new ManualTransactionMachineLookupTableDto
				{
					Id = machine.Id,
					DisplayName = machine.NameEn?.ToString()
				});
			}

            return new PagedResultDto<ManualTransactionMachineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}