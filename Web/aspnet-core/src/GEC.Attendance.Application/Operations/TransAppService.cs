using GEC.Attendance.Authorization.Users;


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

namespace GEC.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_Trans)]
    public class TransAppService : AttendanceAppServiceBase, ITransAppService
    {
		 private readonly IRepository<Tran> _tranRepository;
		 private readonly ITransExcelExporter _transExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public TransAppService(IRepository<Tran> tranRepository, ITransExcelExporter transExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_tranRepository = tranRepository;
			_transExcelExporter = transExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetTranForViewDto>> GetAll(GetAllTransInput input)
         {
			
			var filteredTrans = _tranRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Scan1.Contains(input.Filter) || e.Scan2.Contains(input.Filter) || e.Scan3.Contains(input.Filter) || e.Scan4.Contains(input.Filter) || e.Scan5.Contains(input.Filter) || e.Scan6.Contains(input.Filter) || e.Scan8.Contains(input.Filter) || e.ScanLocation1.Contains(input.Filter) || e.ScanLocation2.Contains(input.Filter) || e.ScanLocation3.Contains(input.Filter) || e.ScanLocation4.Contains(input.Filter) || e.ScanLocation5.Contains(input.Filter) || e.ScanLocation6.Contains(input.Filter) || e.ScanLocation7.Contains(input.Filter) || e.ScanLocation8.Contains(input.Filter) || e.LeaveCode.Contains(input.Filter) || e.LeaveRemark.Contains(input.Filter) || e.ShiftName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan1Filter),  e => e.Scan1 == input.Scan1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan2Filter),  e => e.Scan2 == input.Scan2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan3Filter),  e => e.Scan3 == input.Scan3Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan4Filter),  e => e.Scan4 == input.Scan4Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan5Filter),  e => e.Scan5 == input.Scan5Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan6Filter),  e => e.Scan6 == input.Scan6Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan8Filter),  e => e.Scan8 == input.Scan8Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation1Filter),  e => e.ScanLocation1 == input.ScanLocation1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation2Filter),  e => e.ScanLocation2 == input.ScanLocation2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation3Filter),  e => e.ScanLocation3 == input.ScanLocation3Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation4Filter),  e => e.ScanLocation4 == input.ScanLocation4Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation5Filter),  e => e.ScanLocation5 == input.ScanLocation5Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation6Filter),  e => e.ScanLocation6 == input.ScanLocation6Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation7Filter),  e => e.ScanLocation7 == input.ScanLocation7Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation8Filter),  e => e.ScanLocation8 == input.ScanLocation8Filter)
						.WhereIf(input.HasHolidayFilter > -1,  e => (input.HasHolidayFilter == 1 && e.HasHoliday) || (input.HasHolidayFilter == 0 && !e.HasHoliday) )
						.WhereIf(input.HasVacationFilter > -1,  e => (input.HasVacationFilter == 1 && e.HasVacation) || (input.HasVacationFilter == 0 && !e.HasVacation) )
						.WhereIf(input.HasOffDayFilter > -1,  e => (input.HasOffDayFilter == 1 && e.HasOffDay) || (input.HasOffDayFilter == 0 && !e.HasOffDay) )
						.WhereIf(input.IsAbsentFilter > -1,  e => (input.IsAbsentFilter == 1 && e.IsAbsent) || (input.IsAbsentFilter == 0 && !e.IsAbsent) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveCodeFilter),  e => e.LeaveCode == input.LeaveCodeFilter)
						.WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
						.WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveRemarkFilter),  e => e.LeaveRemark == input.LeaveRemarkFilter)
						.WhereIf(input.MinNoShiftsFilter != null, e => e.NoShifts >= input.MinNoShiftsFilter)
						.WhereIf(input.MaxNoShiftsFilter != null, e => e.NoShifts <= input.MaxNoShiftsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter),  e => e.ShiftName == input.ShiftNameFilter)
						.WhereIf(input.ScanManual1Filter > -1,  e => (input.ScanManual1Filter == 1 && e.ScanManual1) || (input.ScanManual1Filter == 0 && !e.ScanManual1) )
						.WhereIf(input.ScanManual2Filter > -1,  e => (input.ScanManual2Filter == 1 && e.ScanManual2) || (input.ScanManual2Filter == 0 && !e.ScanManual2) )
						.WhereIf(input.ScanManual3Filter > -1,  e => (input.ScanManual3Filter == 1 && e.ScanManual3) || (input.ScanManual3Filter == 0 && !e.ScanManual3) )
						.WhereIf(input.ScanManual4Filter > -1,  e => (input.ScanManual4Filter == 1 && e.ScanManual4) || (input.ScanManual4Filter == 0 && !e.ScanManual4) )
						.WhereIf(input.ScanManual5Filter > -1,  e => (input.ScanManual5Filter == 1 && e.ScanManual5) || (input.ScanManual5Filter == 0 && !e.ScanManual5) )
						.WhereIf(input.ScanManual6Filter > -1,  e => (input.ScanManual6Filter == 1 && e.ScanManual6) || (input.ScanManual6Filter == 0 && !e.ScanManual6) )
						.WhereIf(input.ScanManual7Filter > -1,  e => (input.ScanManual7Filter == 1 && e.ScanManual7) || (input.ScanManual7Filter == 0 && !e.ScanManual7) )
						.WhereIf(input.ScanManual8Filter > -1,  e => (input.ScanManual8Filter == 1 && e.ScanManual8) || (input.ScanManual8Filter == 0 && !e.ScanManual8) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredTrans = filteredTrans
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var trans = from o in pagedAndFilteredTrans
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTranForViewDto() {
							Tran = new TranDto
							{
                                Scan1 = o.Scan1,
                                Scan2 = o.Scan2,
                                Scan3 = o.Scan3,
                                Scan4 = o.Scan4,
                                Scan5 = o.Scan5,
                                Scan6 = o.Scan6,
                                Scan8 = o.Scan8,
                                ScanLocation1 = o.ScanLocation1,
                                ScanLocation2 = o.ScanLocation2,
                                ScanLocation3 = o.ScanLocation3,
                                ScanLocation4 = o.ScanLocation4,
                                ScanLocation5 = o.ScanLocation5,
                                ScanLocation6 = o.ScanLocation6,
                                ScanLocation7 = o.ScanLocation7,
                                ScanLocation8 = o.ScanLocation8,
                                HasHoliday = o.HasHoliday,
                                HasVacation = o.HasVacation,
                                HasOffDay = o.HasOffDay,
                                IsAbsent = o.IsAbsent,
                                LeaveCode = o.LeaveCode,
                                DesignationID = o.DesignationID,
                                LeaveRemark = o.LeaveRemark,
                                NoShifts = o.NoShifts,
                                ShiftName = o.ShiftName,
                                ScanManual1 = o.ScanManual1,
                                ScanManual2 = o.ScanManual2,
                                ScanManual3 = o.ScanManual3,
                                ScanManual4 = o.ScanManual4,
                                ScanManual5 = o.ScanManual5,
                                ScanManual6 = o.ScanManual6,
                                ScanManual7 = o.ScanManual7,
                                ScanManual8 = o.ScanManual8,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredTrans.CountAsync();

            return new PagedResultDto<GetTranForViewDto>(
                totalCount,
                await trans.ToListAsync()
            );
         }
		 
		 public async Task<GetTranForViewDto> GetTranForView(int id)
         {
            var tran = await _tranRepository.GetAsync(id);

            var output = new GetTranForViewDto { Tran = ObjectMapper.Map<TranDto>(tran) };

		    if (output.Tran.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Tran.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Trans_Edit)]
		 public async Task<GetTranForEditOutput> GetTranForEdit(EntityDto input)
         {
            var tran = await _tranRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTranForEditOutput {Tran = ObjectMapper.Map<CreateOrEditTranDto>(tran)};

		    if (output.Tran.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Tran.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTranDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Trans_Create)]
		 protected virtual async Task Create(CreateOrEditTranDto input)
         {
            var tran = ObjectMapper.Map<Tran>(input);

			

            await _tranRepository.InsertAsync(tran);
         }

		 [AbpAuthorize(AppPermissions.Pages_Trans_Edit)]
		 protected virtual async Task Update(CreateOrEditTranDto input)
         {
            var tran = await _tranRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, tran);
         }

		 [AbpAuthorize(AppPermissions.Pages_Trans_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _tranRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTransToExcel(GetAllTransForExcelInput input)
         {
			
			var filteredTrans = _tranRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Scan1.Contains(input.Filter) || e.Scan2.Contains(input.Filter) || e.Scan3.Contains(input.Filter) || e.Scan4.Contains(input.Filter) || e.Scan5.Contains(input.Filter) || e.Scan6.Contains(input.Filter) || e.Scan8.Contains(input.Filter) || e.ScanLocation1.Contains(input.Filter) || e.ScanLocation2.Contains(input.Filter) || e.ScanLocation3.Contains(input.Filter) || e.ScanLocation4.Contains(input.Filter) || e.ScanLocation5.Contains(input.Filter) || e.ScanLocation6.Contains(input.Filter) || e.ScanLocation7.Contains(input.Filter) || e.ScanLocation8.Contains(input.Filter) || e.LeaveCode.Contains(input.Filter) || e.LeaveRemark.Contains(input.Filter) || e.ShiftName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan1Filter),  e => e.Scan1 == input.Scan1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan2Filter),  e => e.Scan2 == input.Scan2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan3Filter),  e => e.Scan3 == input.Scan3Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan4Filter),  e => e.Scan4 == input.Scan4Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan5Filter),  e => e.Scan5 == input.Scan5Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan6Filter),  e => e.Scan6 == input.Scan6Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Scan8Filter),  e => e.Scan8 == input.Scan8Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation1Filter),  e => e.ScanLocation1 == input.ScanLocation1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation2Filter),  e => e.ScanLocation2 == input.ScanLocation2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation3Filter),  e => e.ScanLocation3 == input.ScanLocation3Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation4Filter),  e => e.ScanLocation4 == input.ScanLocation4Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation5Filter),  e => e.ScanLocation5 == input.ScanLocation5Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation6Filter),  e => e.ScanLocation6 == input.ScanLocation6Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation7Filter),  e => e.ScanLocation7 == input.ScanLocation7Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation8Filter),  e => e.ScanLocation8 == input.ScanLocation8Filter)
						.WhereIf(input.HasHolidayFilter > -1,  e => (input.HasHolidayFilter == 1 && e.HasHoliday) || (input.HasHolidayFilter == 0 && !e.HasHoliday) )
						.WhereIf(input.HasVacationFilter > -1,  e => (input.HasVacationFilter == 1 && e.HasVacation) || (input.HasVacationFilter == 0 && !e.HasVacation) )
						.WhereIf(input.HasOffDayFilter > -1,  e => (input.HasOffDayFilter == 1 && e.HasOffDay) || (input.HasOffDayFilter == 0 && !e.HasOffDay) )
						.WhereIf(input.IsAbsentFilter > -1,  e => (input.IsAbsentFilter == 1 && e.IsAbsent) || (input.IsAbsentFilter == 0 && !e.IsAbsent) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveCodeFilter),  e => e.LeaveCode == input.LeaveCodeFilter)
						.WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
						.WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveRemarkFilter),  e => e.LeaveRemark == input.LeaveRemarkFilter)
						.WhereIf(input.MinNoShiftsFilter != null, e => e.NoShifts >= input.MinNoShiftsFilter)
						.WhereIf(input.MaxNoShiftsFilter != null, e => e.NoShifts <= input.MaxNoShiftsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameFilter),  e => e.ShiftName == input.ShiftNameFilter)
						.WhereIf(input.ScanManual1Filter > -1,  e => (input.ScanManual1Filter == 1 && e.ScanManual1) || (input.ScanManual1Filter == 0 && !e.ScanManual1) )
						.WhereIf(input.ScanManual2Filter > -1,  e => (input.ScanManual2Filter == 1 && e.ScanManual2) || (input.ScanManual2Filter == 0 && !e.ScanManual2) )
						.WhereIf(input.ScanManual3Filter > -1,  e => (input.ScanManual3Filter == 1 && e.ScanManual3) || (input.ScanManual3Filter == 0 && !e.ScanManual3) )
						.WhereIf(input.ScanManual4Filter > -1,  e => (input.ScanManual4Filter == 1 && e.ScanManual4) || (input.ScanManual4Filter == 0 && !e.ScanManual4) )
						.WhereIf(input.ScanManual5Filter > -1,  e => (input.ScanManual5Filter == 1 && e.ScanManual5) || (input.ScanManual5Filter == 0 && !e.ScanManual5) )
						.WhereIf(input.ScanManual6Filter > -1,  e => (input.ScanManual6Filter == 1 && e.ScanManual6) || (input.ScanManual6Filter == 0 && !e.ScanManual6) )
						.WhereIf(input.ScanManual7Filter > -1,  e => (input.ScanManual7Filter == 1 && e.ScanManual7) || (input.ScanManual7Filter == 0 && !e.ScanManual7) )
						.WhereIf(input.ScanManual8Filter > -1,  e => (input.ScanManual8Filter == 1 && e.ScanManual8) || (input.ScanManual8Filter == 0 && !e.ScanManual8) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredTrans
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTranForViewDto() { 
							Tran = new TranDto
							{
                                Scan1 = o.Scan1,
                                Scan2 = o.Scan2,
                                Scan3 = o.Scan3,
                                Scan4 = o.Scan4,
                                Scan5 = o.Scan5,
                                Scan6 = o.Scan6,
                                Scan8 = o.Scan8,
                                ScanLocation1 = o.ScanLocation1,
                                ScanLocation2 = o.ScanLocation2,
                                ScanLocation3 = o.ScanLocation3,
                                ScanLocation4 = o.ScanLocation4,
                                ScanLocation5 = o.ScanLocation5,
                                ScanLocation6 = o.ScanLocation6,
                                ScanLocation7 = o.ScanLocation7,
                                ScanLocation8 = o.ScanLocation8,
                                HasHoliday = o.HasHoliday,
                                HasVacation = o.HasVacation,
                                HasOffDay = o.HasOffDay,
                                IsAbsent = o.IsAbsent,
                                LeaveCode = o.LeaveCode,
                                DesignationID = o.DesignationID,
                                LeaveRemark = o.LeaveRemark,
                                NoShifts = o.NoShifts,
                                ShiftName = o.ShiftName,
                                ScanManual1 = o.ScanManual1,
                                ScanManual2 = o.ScanManual2,
                                ScanManual3 = o.ScanManual3,
                                ScanManual4 = o.ScanManual4,
                                ScanManual5 = o.ScanManual5,
                                ScanManual6 = o.ScanManual6,
                                ScanManual7 = o.ScanManual7,
                                ScanManual8 = o.ScanManual8,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var tranListDtos = await query.ToListAsync();

            return _transExcelExporter.ExportToFile(tranListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Trans)]
         public async Task<PagedResultDto<TranUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TranUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new TranUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<TranUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}