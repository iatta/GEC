using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.Attendance.Setting.Exporting;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.Attendance.Setting
{
	[AbpAuthorize(AppPermissions.Pages_OverrideShifts)]
    public class OverrideShiftsAppService : AttendanceAppServiceBase, IOverrideShiftsAppService
    {
		 private readonly IRepository<OverrideShift> _overrideShiftRepository;
		 private readonly IOverrideShiftsExcelExporter _overrideShiftsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Shift,int> _lookup_shiftRepository;
		 

		  public OverrideShiftsAppService(IRepository<OverrideShift> overrideShiftRepository, IOverrideShiftsExcelExporter overrideShiftsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Shift, int> lookup_shiftRepository) 
		  {
			_overrideShiftRepository = overrideShiftRepository;
			_overrideShiftsExcelExporter = overrideShiftsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_shiftRepository = lookup_shiftRepository;
		
		  }

		 public async Task<PagedResultDto<GetOverrideShiftForViewDto>> GetAll(GetAllOverrideShiftsInput input)
         {
			
			var filteredOverrideShifts = _overrideShiftRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinDayFilter != null, e => e.Day >= input.MinDayFilter)
						.WhereIf(input.MaxDayFilter != null, e => e.Day <= input.MaxDayFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameEnFilter), e => e.ShiftFk != null && e.ShiftFk.NameEn == input.ShiftNameEnFilter);

			var pagedAndFilteredOverrideShifts = filteredOverrideShifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var overrideShifts = from o in pagedAndFilteredOverrideShifts
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetOverrideShiftForViewDto() {
							OverrideShift = new OverrideShiftDto
							{
                                Day = o.Day,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
						};

            var totalCount = await filteredOverrideShifts.CountAsync();

            return new PagedResultDto<GetOverrideShiftForViewDto>(
                totalCount,
                await overrideShifts.ToListAsync()
            );
         }
		 
		 public async Task<GetOverrideShiftForViewDto> GetOverrideShiftForView(int id)
         {
            var overrideShift = await _overrideShiftRepository.GetAsync(id);

            var output = new GetOverrideShiftForViewDto { OverrideShift = ObjectMapper.Map<OverrideShiftDto>(overrideShift) };

		    if (output.OverrideShift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OverrideShift.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.OverrideShift.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.OverrideShift.ShiftId);
                output.ShiftNameEn = _lookupShift.NameEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_OverrideShifts_Edit)]
		 public async Task<GetOverrideShiftForEditOutput> GetOverrideShiftForEdit(EntityDto input)
         {
            var overrideShift = await _overrideShiftRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetOverrideShiftForEditOutput {OverrideShift = ObjectMapper.Map<CreateOrEditOverrideShiftDto>(overrideShift)};

		    if (output.OverrideShift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OverrideShift.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.OverrideShift.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.OverrideShift.ShiftId);
                output.ShiftNameEn = _lookupShift.NameEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditOverrideShiftDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_OverrideShifts_Create)]
		 protected virtual async Task Create(CreateOrEditOverrideShiftDto input)
         {
            var overrideShift = ObjectMapper.Map<OverrideShift>(input);

			

            await _overrideShiftRepository.InsertAsync(overrideShift);
         }

		 [AbpAuthorize(AppPermissions.Pages_OverrideShifts_Edit)]
		 protected virtual async Task Update(CreateOrEditOverrideShiftDto input)
         {
            var overrideShift = await _overrideShiftRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, overrideShift);
         }

		 [AbpAuthorize(AppPermissions.Pages_OverrideShifts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _overrideShiftRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetOverrideShiftsToExcel(GetAllOverrideShiftsForExcelInput input)
         {
			
			var filteredOverrideShifts = _overrideShiftRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinDayFilter != null, e => e.Day >= input.MinDayFilter)
						.WhereIf(input.MaxDayFilter != null, e => e.Day <= input.MaxDayFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameEnFilter), e => e.ShiftFk != null && e.ShiftFk.NameEn == input.ShiftNameEnFilter);

			var query = (from o in filteredOverrideShifts
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetOverrideShiftForViewDto() { 
							OverrideShift = new OverrideShiftDto
							{
                                Day = o.Day,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
						 });


            var overrideShiftListDtos = await query.ToListAsync();

            return _overrideShiftsExcelExporter.ExportToFile(overrideShiftListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_OverrideShifts)]
         public async Task<PagedResultDto<OverrideShiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<OverrideShiftUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new OverrideShiftUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<OverrideShiftUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_OverrideShifts)]
         public async Task<PagedResultDto<OverrideShiftShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<OverrideShiftShiftLookupTableDto>();
			foreach(var shift in shiftList){
				lookupTableDtoList.Add(new OverrideShiftShiftLookupTableDto
				{
					Id = shift.Id,
					DisplayName = shift.NameEn?.ToString()
				});
			}

            return new PagedResultDto<OverrideShiftShiftLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}