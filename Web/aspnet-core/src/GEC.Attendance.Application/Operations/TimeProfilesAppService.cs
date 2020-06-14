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
using GEC.Attendance.Setting;
using System.Transactions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;

namespace GEC.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_TimeProfiles)]
    public class TimeProfilesAppService : AttendanceAppServiceBase, ITimeProfilesAppService
    {
		 private readonly IRepository<TimeProfile> _timeProfileRepository;
         private readonly IRepository<Shift> _shiftRepository;
         private readonly IRepository<ShiftType> _shiftTypeRepository;
         private readonly UserManager _userManager;
         private readonly ITimeProfilesExcelExporter _timeProfilesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public TimeProfilesAppService(IRepository<TimeProfile> timeProfileRepository, 
              ITimeProfilesExcelExporter timeProfilesExcelExporter ,
              IRepository<User, long> lookup_userRepository ,
              IRepository<Shift> shiftRepository,
              IRepository<ShiftType> shiftTypeRepository,
              UserManager userManager
              ) 
		  {
			_timeProfileRepository = timeProfileRepository;
			_timeProfilesExcelExporter = timeProfilesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
            _shiftRepository = shiftRepository;
            _shiftTypeRepository = shiftTypeRepository;
            _userManager = userManager;



          }

		 public async Task<PagedResultDto<GetTimeProfileForViewDto>> GetAll(GetAllTimeProfilesInput input)
         {
			
			var filteredTimeProfiles = _timeProfileRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DescriptionAr.Contains(input.Filter) || e.DescriptionEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredTimeProfiles = filteredTimeProfiles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var timeProfiles = from o in pagedAndFilteredTimeProfiles
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTimeProfileForViewDto() {
							TimeProfile = new TimeProfileDto
							{
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredTimeProfiles.CountAsync();

            return new PagedResultDto<GetTimeProfileForViewDto>(
                totalCount,
                await timeProfiles.ToListAsync()
            );
         }
		 
		 public async Task<GetTimeProfileForViewDto> GetTimeProfileForView(int id)
         {
            var timeProfile = await _timeProfileRepository.GetAsync(id);

            var output = new GetTimeProfileForViewDto { TimeProfile = ObjectMapper.Map<TimeProfileDto>(timeProfile) };

		    if (output.TimeProfile.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TimeProfile.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TimeProfiles_Edit)]
		 public async Task<GetTimeProfileForEditOutput> GetTimeProfileForEdit(EntityDto input)
         {
            var timeProfile = await _timeProfileRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTimeProfileForEditOutput {TimeProfile = ObjectMapper.Map<CreateOrEditTimeProfileDto>(timeProfile)};

		    if (output.TimeProfile.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TimeProfile.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

        public async Task CreateTimeProfileFromExcel(List<CreateTimeProfileFromExcelDto> input)
        {
            try
            {
                using (var uom = UnitOfWorkManager.Begin())
                {
                    foreach (var item in input)
                    {
                        var timeProfileToAdd = new CreateOrEditTimeProfileDto();
                        var shiftid = _shiftRepository.FirstOrDefault(x => x.NameAr.Contains(item.Shift)).Id;
                        var shiftTypeid = _shiftTypeRepository.FirstOrDefault(x => x.DescriptionAr.Contains(item.ShiftType)).Id;
                        var userId = _userManager.Users.Where(x => x.Code == item.EmployeeCode).FirstOrDefault().Id;
                        if (shiftTypeid != 0 && shiftid != 0 && userId != 0)
                        {
                            timeProfileToAdd.ShiftTypeID_Saturday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Sunday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Monday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Tuesday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Wednesday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Thursday = shiftTypeid;
                            timeProfileToAdd.ShiftTypeID_Friday = shiftTypeid;
                            timeProfileToAdd.UserId = userId;
                            timeProfileToAdd.StartDate = item.Date;
                            timeProfileToAdd.EndDate = item.Date.AddDays(7);

                            for (int i = 1; i <= 7; i++)
                            {
                                var timeProfileDetail = new TimeProfileDetailDto();
                                timeProfileDetail.ShiftId = shiftid;
                                timeProfileDetail.Day = i;
                                timeProfileToAdd.TimeProfileDetails.Add(timeProfileDetail);
                            }

                            var timeProfile = ObjectMapper.Map<TimeProfile>(timeProfileToAdd);

                            await _timeProfileRepository.InsertAsync(timeProfile);

                        }

                    }

                    uom.Complete();
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("failed");
            }

               
           

        }
       


         public async Task CreateOrEdit(List<CreateOrEditTimeProfileDto> input)
         {
            foreach (var item in input)
            {
                if (item.Id == null)
                    await Create(item);
                else
                    await Update(item);
            }
            
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfiles_Create)]
		 protected virtual async Task Create(CreateOrEditTimeProfileDto input)
         {
            var timeProfile = ObjectMapper.Map<TimeProfile>(input);

			

            await _timeProfileRepository.InsertAsync(timeProfile);
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfiles_Edit)]
		 protected virtual async Task Update(CreateOrEditTimeProfileDto input)
         {
            var timeProfile = await _timeProfileRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, timeProfile);
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfiles_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _timeProfileRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTimeProfilesToExcel(GetAllTimeProfilesForExcelInput input)
         {
			
			var filteredTimeProfiles = _timeProfileRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DescriptionAr.Contains(input.Filter) || e.DescriptionEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredTimeProfiles
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTimeProfileForViewDto() { 
							TimeProfile = new TimeProfileDto
							{
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var timeProfileListDtos = await query.ToListAsync();

            return _timeProfilesExcelExporter.ExportToFile(timeProfileListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TimeProfiles)]
         public async Task<PagedResultDto<TimeProfileUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TimeProfileUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new TimeProfileUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<TimeProfileUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}