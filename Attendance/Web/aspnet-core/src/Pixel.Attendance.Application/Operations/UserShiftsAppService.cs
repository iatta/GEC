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
using Twilio.Exceptions;

namespace Pixel.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_UserShifts)]
    public class UserShiftsAppService : AttendanceAppServiceBase, IUserShiftsAppService
    {
		 private readonly IRepository<UserShift> _userShiftRepository;
		 private readonly IUserShiftsExcelExporter _userShiftsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Shift,int> _lookup_shiftRepository;
		 

		  public UserShiftsAppService(IRepository<UserShift> userShiftRepository, IUserShiftsExcelExporter userShiftsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Shift, int> lookup_shiftRepository) 
		  {
			_userShiftRepository = userShiftRepository;
			_userShiftsExcelExporter = userShiftsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_shiftRepository = lookup_shiftRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserShiftForViewDto>> GetAll(GetAllUserShiftsInput input)
         {
			
			var filteredUserShifts = _userShiftRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameEnFilter), e => e.ShiftFk != null && e.ShiftFk.NameEn == input.ShiftNameEnFilter);

			var pagedAndFilteredUserShifts = filteredUserShifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userShifts = from o in pagedAndFilteredUserShifts
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserShiftForViewDto() {
							UserShift = new UserShiftDto
							{
                                Date = o.Date,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
						};

            var totalCount = await filteredUserShifts.CountAsync();

            return new PagedResultDto<GetUserShiftForViewDto>(
                totalCount,
                await userShifts.ToListAsync()
            );
         }
		 
		 public async Task<GetUserShiftForViewDto> GetUserShiftForView(int id)
         {
            var userShift = await _userShiftRepository.GetAsync(id);

            var output = new GetUserShiftForViewDto { UserShift = ObjectMapper.Map<UserShiftDto>(userShift) };

		    if (output.UserShift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserShift.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.UserShift.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.UserShift.ShiftId);
                output.ShiftNameEn = _lookupShift.NameEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UserShifts_Edit)]
		 public async Task<GetUserShiftForEditOutput> GetUserShiftForEdit(EntityDto input)
         {
            var userShift = await _userShiftRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserShiftForEditOutput {UserShift = ObjectMapper.Map<CreateOrEditUserShiftDto>(userShift)};

		    if (output.UserShift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserShift.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.UserShift.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.UserShift.ShiftId);
                output.ShiftNameEn = _lookupShift.NameEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserShiftDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_UserShifts_Create)]
		 protected virtual async Task Create(CreateOrEditUserShiftDto input)
         {
            var userShift = ObjectMapper.Map<UserShift>(input);

			

            await _userShiftRepository.InsertAsync(userShift);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserShifts_Edit)]
		 protected virtual async Task Update(CreateOrEditUserShiftDto input)
         {
            var userShift = await _userShiftRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, userShift);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserShifts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _userShiftRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserShiftsToExcel(GetAllUserShiftsForExcelInput input)
         {
			
			var filteredUserShifts = _userShiftRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameEnFilter), e => e.ShiftFk != null && e.ShiftFk.NameEn == input.ShiftNameEnFilter);

			var query = (from o in filteredUserShifts
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserShiftForViewDto() { 
							UserShift = new UserShiftDto
							{
                                Date = o.Date,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
						 });


            var userShiftListDtos = await query.ToListAsync();

            return _userShiftsExcelExporter.ExportToFile(userShiftListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UserShifts)]
         public async Task<PagedResultDto<UserShiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserShiftUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserShiftUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserShiftUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_UserShifts)]
         public async Task<PagedResultDto<UserShiftShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserShiftShiftLookupTableDto>();
			foreach(var shift in shiftList){
				lookupTableDtoList.Add(new UserShiftShiftLookupTableDto
				{
					Id = shift.Id,
					DisplayName = shift.NameEn?.ToString()
				});
			}

            return new PagedResultDto<UserShiftShiftLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task BulkCreateOrEdit(List<CreateOrEditUserShiftDto> input)
        {
            if (input.Count == 0)
                throw new ApiException("Please Select Employees");
            
            foreach (var userShift in input)
            {
                
                //delete if exist
                var existUserShift = await _userShiftRepository.FirstOrDefaultAsync(x => x.Date.Date == userShift.Date.Date  && x.UserId == userShift.UserId && x.ShiftId == userShift.ShiftId);
                if (existUserShift == null)
                {
                    var newUserShift = ObjectMapper.Map<UserShift>(userShift);
                    await _userShiftRepository.InsertAsync(newUserShift);
                }
            }

        }
    }
}