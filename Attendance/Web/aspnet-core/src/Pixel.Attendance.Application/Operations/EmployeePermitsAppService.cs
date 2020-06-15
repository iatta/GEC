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
using Abp.UI;

namespace Pixel.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_EmployeePermits)]
    public class EmployeePermitsAppService : AttendanceAppServiceBase, IEmployeePermitsAppService
    {
		 private readonly IRepository<EmployeePermit> _employeePermitRepository;
		 private readonly IEmployeePermitsExcelExporter _employeePermitsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Permit,int> _lookup_permitRepository;
		 

		  public EmployeePermitsAppService(IRepository<EmployeePermit> employeePermitRepository, IEmployeePermitsExcelExporter employeePermitsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Permit, int> lookup_permitRepository) 
		  {
			_employeePermitRepository = employeePermitRepository;
			_employeePermitsExcelExporter = employeePermitsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_permitRepository = lookup_permitRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeePermitForViewDto>> GetAll(GetAllEmployeePermitsInput input)
         {
			
			var filteredEmployeePermits = _employeePermitRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.PermitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(input.MinPermitDateFilter != null, e => e.PermitDate >= input.MinPermitDateFilter)
						.WhereIf(input.MaxPermitDateFilter != null, e => e.PermitDate <= input.MaxPermitDateFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PermitDescriptionArFilter), e => e.PermitFk != null && e.PermitFk.DescriptionAr == input.PermitDescriptionArFilter)
                        .Where(e => e.UserId == GetCurrentUser().Id);

            var pagedAndFilteredEmployeePermits = filteredEmployeePermits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeePermits = from o in pagedAndFilteredEmployeePermits
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_permitRepository.GetAll() on o.PermitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeePermitForViewDto() {
							EmployeePermit = new EmployeePermitDto
							{
                                //FromTime = o.FromTime,
                                //ToTime = o.ToTime,
                                PermitDate = o.PermitDate,
                                Description = o.Description,
                                Status = o.Status,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	PermitDescriptionAr = s2 == null ? "" : s2.DescriptionAr.ToString()
						};

            var totalCount = await filteredEmployeePermits.CountAsync();

            return new PagedResultDto<GetEmployeePermitForViewDto>(
                totalCount,
                await employeePermits.ToListAsync()
            );
         }

        public async Task<PagedResultDto<GetEmployeePermitForViewDto>> getAllForManager(GetAllEmployeePermitsInput input)
        {

            var filteredEmployeePermits = _employeePermitRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.PermitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Note.Contains(input.Filter))
                        .WhereIf(input.MinPermitDateFilter != null, e => e.PermitDate >= input.MinPermitDateFilter)
                        .WhereIf(input.MaxPermitDateFilter != null, e => e.PermitDate <= input.MaxPermitDateFilter)
                        .WhereIf(input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PermitDescriptionArFilter), e => e.PermitFk != null && e.PermitFk.DescriptionAr == input.PermitDescriptionArFilter)
                        .Where(e => e.UserFk != null && e.UserFk.ManagerId == GetCurrentUser().Id);

            var pagedAndFilteredEmployeePermits = filteredEmployeePermits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeePermits = from o in pagedAndFilteredEmployeePermits
                                  join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_permitRepository.GetAll() on o.PermitId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new GetEmployeePermitForViewDto()
                                  {
                                      EmployeePermit = new EmployeePermitDto
                                      {
                                          //FromTime = o.FromTime,
                                          //ToTime = o.ToTime,
                                          PermitDate = o.PermitDate,
                                          Description = o.Description,
                                          Status = o.Status,
                                          Id = o.Id
                                      },
                                      UserName = s1 == null ? "" : s1.Name.ToString(),
                                      PermitDescriptionAr = s2 == null ? "" : s2.DescriptionAr.ToString()
                                  };

            var totalCount = await filteredEmployeePermits.CountAsync();

            return new PagedResultDto<GetEmployeePermitForViewDto>(
                totalCount,
                await employeePermits.ToListAsync()
            );
        }


        public async Task<GetEmployeePermitForViewDto> GetEmployeePermitForView(int id)
         {
            var employeePermit = await _employeePermitRepository.GetAsync(id);

            var output = new GetEmployeePermitForViewDto { EmployeePermit = ObjectMapper.Map<EmployeePermitDto>(employeePermit) };

		    if (output.EmployeePermit.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeePermit.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeePermit.PermitId != null)
            {
                var _lookupPermit = await _lookup_permitRepository.FirstOrDefaultAsync((int)output.EmployeePermit.PermitId);
                output.PermitDescriptionAr = _lookupPermit.DescriptionAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeePermits_Edit)]
		 public async Task<GetEmployeePermitForEditOutput> GetEmployeePermitForEdit(EntityDto input)
         {
            var employeePermit = await _employeePermitRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeePermitForEditOutput {EmployeePermit = ObjectMapper.Map<CreateOrEditEmployeePermitDto>(employeePermit)};

		    if (output.EmployeePermit.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeePermit.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeePermit.PermitId != null)
            {
                var _lookupPermit = await _lookup_permitRepository.FirstOrDefaultAsync((int)output.EmployeePermit.PermitId);
                output.PermitDescriptionAr = _lookupPermit.DescriptionAr.ToString();
            }
			
            return output;
         }

        private async Task ValidateEmployeePermit(CreateOrEditEmployeePermitDto input)
        {
            var permit = await  _lookup_permitRepository.FirstOrDefaultAsync(x => x.Id == input.PermitId);
            var timeReuested = (input.ToTime/60) - (input.FromTime/60);

            var permitsDay =input.Id == null ? await _employeePermitRepository.GetAllListAsync(x => x.PermitDate == input.PermitDate) : await _employeePermitRepository.GetAllListAsync(x => x.PermitDate == input.PermitDate && x.PermitId != input.PermitId);

            int days = input.PermitDate.DayOfWeek - DayOfWeek.Sunday;
            var permitDate = input.PermitDate;
            DateTime weekStart = permitDate.AddDays(-days);
            DateTime weekEnd =   permitDate.AddDays(6);

            var permitsWeek = input.Id == null ? await _employeePermitRepository.GetAllListAsync(x => x.PermitDate <= weekEnd && x.PermitDate >=  weekStart) : await _employeePermitRepository.GetAllListAsync(x => (x.PermitDate <= weekEnd && x.PermitDate >= weekStart) && x.PermitId != input.PermitId);
            var permitsMonth = input.Id == null ?  await _employeePermitRepository.GetAllListAsync(x => x.PermitDate.Month == input.PermitDate.Month) : await _employeePermitRepository.GetAllListAsync(x => (x.PermitDate.Month == input.PermitDate.Month) && x.PermitId != input.PermitId);


            #region Check allow time
            //check time per day 
            var totalEmployeePermitsHourPerDay = permitsDay.Select(x => (x.ToTime - x.FromTime) / 60).Sum();
            var allowedHoursPerDay = permit.TotalHoursPerDay - totalEmployeePermitsHourPerDay;
            if (timeReuested > allowedHoursPerDay)
                throw new UserFriendlyException(L($"Sorry only {permit.TotalHoursPerDay} hour/hours are Allowed per day"));



            //check time per week 
            var totalEmployeePermitsHourPerWeek = permitsWeek.Select(x => (x.ToTime - x.FromTime) / 60).Sum();
            var allowedHoursPerWeek = permit.TotalHoursPerWeek - totalEmployeePermitsHourPerWeek;
            if (timeReuested > allowedHoursPerWeek)
                throw new UserFriendlyException(L($"Sorry only {permit.TotalHoursPerDay} hour/hours are Allowed per week"));


            //check time per month
            var totalEmployeePermitsHourPerMonth = permitsMonth.Select(x => (x.ToTime - x.FromTime) / 60).Sum();
            var allowedHoursPerMonth = permit.TotalHoursPerMonth - totalEmployeePermitsHourPerMonth;
            if (timeReuested > allowedHoursPerMonth)
                throw new UserFriendlyException(L($"Sorry only {permit.TotalHoursPerDay} hour/hours are Allowed per month"));

            #endregion


            #region Check total count
            //check time per week
            var empPermitsPerDay = permitsDay.Count;
            
            if (empPermitsPerDay >= permit.MaxNumberPerDay  )
                throw new UserFriendlyException(L($"Sorry only {permit.MaxNumberPerDay} Permits are Allowed per day"));


            //check time per week
            var empPermitsPerWeek = permitsWeek.Count;

            //if (empPermitsPerDay >= int.Parse(permit.MaxNumberPerWeek))
            //    throw new UserFriendlyException(L($"Sorry only {permit.MaxNumberPerWeek} hour/hours are Allowed"));


            //check time per month
            var empPermitsPerMonth = permitsMonth.Count;
            if (empPermitsPerMonth >= permit.MaxNumberPerMonth)
                throw new UserFriendlyException(L($"Sorry only {permit.MaxNumberPerMonth} hour/hours are Allowed"));
            #endregion
          
        }

        public async Task CreateOrEdit(CreateOrEditEmployeePermitDto input)
         {

            await ValidateEmployeePermit(input);
            
            if (input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeePermits_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeePermitDto input)
         {

            input.UserId = GetCurrentUser().Id;
            var employeePermit = ObjectMapper.Map<EmployeePermit>(input);

			

            await _employeePermitRepository.InsertAsync(employeePermit);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeePermits_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeePermitDto input)
         {
            var employeePermit = await _employeePermitRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeePermit);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeePermits_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeePermitRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeePermitsToExcel(GetAllEmployeePermitsForExcelInput input)
         {
			
			var filteredEmployeePermits = _employeePermitRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.PermitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(input.MinPermitDateFilter != null, e => e.PermitDate >= input.MinPermitDateFilter)
						.WhereIf(input.MaxPermitDateFilter != null, e => e.PermitDate <= input.MaxPermitDateFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PermitDescriptionArFilter), e => e.PermitFk != null && e.PermitFk.DescriptionAr == input.PermitDescriptionArFilter);

			var query = (from o in filteredEmployeePermits
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_permitRepository.GetAll() on o.PermitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeePermitForViewDto() { 
							EmployeePermit = new EmployeePermitDto
							{
                                //FromTime = o.FromTime,
                                PermitDate = o.PermitDate,
                                Description = o.Description,
                                Status = o.Status,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	PermitDescriptionAr = s2 == null ? "" : s2.DescriptionAr.ToString()
						 });


            var employeePermitListDtos = await query.ToListAsync();

            return _employeePermitsExcelExporter.ExportToFile(employeePermitListDtos);
         }

       



		[AbpAuthorize(AppPermissions.Pages_EmployeePermits)]
         public async Task<PagedResultDto<EmployeePermitUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeePermitUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new EmployeePermitUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeePermitUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_EmployeePermits)]
         public async Task<PagedResultDto<EmployeePermitPermitLookupTableDto>> GetAllPermitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_permitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DescriptionAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var permitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeePermitPermitLookupTableDto>();
			foreach(var permit in permitList){
				lookupTableDtoList.Add(new EmployeePermitPermitLookupTableDto
				{
					Id = permit.Id,
					DisplayName = permit.DescriptionAr?.ToString()
				});
			}

            return new PagedResultDto<EmployeePermitPermitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}