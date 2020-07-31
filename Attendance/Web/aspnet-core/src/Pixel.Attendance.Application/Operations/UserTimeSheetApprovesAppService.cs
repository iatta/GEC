using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Operations;


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
	[AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves)]
    public class UserTimeSheetApprovesAppService : AttendanceAppServiceBase, IUserTimeSheetApprovesAppService
    {
		 private readonly IRepository<UserTimeSheetApprove> _userTimeSheetApproveRepository;
		 private readonly IUserTimeSheetApprovesExcelExporter _userTimeSheetApprovesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Project,int> _lookup_projectRepository;
		 

		  public UserTimeSheetApprovesAppService(IRepository<UserTimeSheetApprove> userTimeSheetApproveRepository, IUserTimeSheetApprovesExcelExporter userTimeSheetApprovesExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Project, int> lookup_projectRepository) 
		  {
			_userTimeSheetApproveRepository = userTimeSheetApproveRepository;
			_userTimeSheetApprovesExcelExporter = userTimeSheetApprovesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_projectRepository = lookup_projectRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserTimeSheetApproveForViewDto>> GetAll(GetAllUserTimeSheetApprovesInput input)
         {
			
			var filteredUserTimeSheetApproves = _userTimeSheetApproveRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ProjectManagerFk)
						.Include( e => e.ProjectFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ApprovedUnits.Contains(input.Filter))
						.WhereIf(input.MinMonthFilter != null, e => e.Month >= input.MinMonthFilter)
						.WhereIf(input.MaxMonthFilter != null, e => e.Month <= input.MaxMonthFilter)
						.WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
						.WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedUnitsFilter),  e => e.ApprovedUnits == input.ApprovedUnitsFilter)
						.WhereIf(input.ProjectManagerApproveFilter > -1,  e => (input.ProjectManagerApproveFilter == 1 && e.ProjectManagerApprove) || (input.ProjectManagerApproveFilter == 0 && !e.ProjectManagerApprove) )
						.WhereIf(input.IsClosedFilter > -1,  e => (input.IsClosedFilter == 1 && e.IsClosed) || (input.IsClosedFilter == 0 && !e.IsClosed) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.ProjectManagerFk != null && e.ProjectManagerFk.Name == input.UserName2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameEnFilter), e => e.ProjectFk != null && e.ProjectFk.NameEn == input.ProjectNameEnFilter);

			var pagedAndFilteredUserTimeSheetApproves = filteredUserTimeSheetApproves
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userTimeSheetApproves = from o in pagedAndFilteredUserTimeSheetApproves
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.ProjectManagerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_projectRepository.GetAll() on o.ProjectId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetUserTimeSheetApproveForViewDto() {
							UserTimeSheetApprove = new UserTimeSheetApproveDto
							{
                                Month = o.Month,
                                Year = o.Year,
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                ApprovedUnits = o.ApprovedUnits,
                                ProjectManagerApprove = o.ProjectManagerApprove,
                                IsClosed = o.IsClosed,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName2 = s2 == null ? "" : s2.Name.ToString(),
                         	ProjectNameEn = s3 == null ? "" : s3.NameEn.ToString()
						};

            var totalCount = await filteredUserTimeSheetApproves.CountAsync();

            return new PagedResultDto<GetUserTimeSheetApproveForViewDto>(
                totalCount,
                await userTimeSheetApproves.ToListAsync()
            );
         }
		 
		 public async Task<GetUserTimeSheetApproveForViewDto> GetUserTimeSheetApproveForView(int id)
         {
            var userTimeSheetApprove = await _userTimeSheetApproveRepository.GetAsync(id);

            var output = new GetUserTimeSheetApproveForViewDto { UserTimeSheetApprove = ObjectMapper.Map<UserTimeSheetApproveDto>(userTimeSheetApprove) };

		    if (output.UserTimeSheetApprove.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserTimeSheetApprove.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.UserTimeSheetApprove.ProjectManagerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserTimeSheetApprove.ProjectManagerId);
                output.UserName2 = _lookupUser.Name.ToString();
            }

		    if (output.UserTimeSheetApprove.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.UserTimeSheetApprove.ProjectId);
                output.ProjectNameEn = _lookupProject.NameEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves_Edit)]
		 public async Task<GetUserTimeSheetApproveForEditOutput> GetUserTimeSheetApproveForEdit(EntityDto input)
         {
            var userTimeSheetApprove = await _userTimeSheetApproveRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserTimeSheetApproveForEditOutput {UserTimeSheetApprove = ObjectMapper.Map<CreateOrEditUserTimeSheetApproveDto>(userTimeSheetApprove)};

		    if (output.UserTimeSheetApprove.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserTimeSheetApprove.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.UserTimeSheetApprove.ProjectManagerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserTimeSheetApprove.ProjectManagerId);
                output.UserName2 = _lookupUser.Name.ToString();
            }

		    if (output.UserTimeSheetApprove.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.UserTimeSheetApprove.ProjectId);
                output.ProjectNameEn = _lookupProject.NameEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserTimeSheetApproveDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves_Create)]
		 protected virtual async Task Create(CreateOrEditUserTimeSheetApproveDto input)
         {
            var userTimeSheetApprove = ObjectMapper.Map<UserTimeSheetApprove>(input);

			

            await _userTimeSheetApproveRepository.InsertAsync(userTimeSheetApprove);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves_Edit)]
		 protected virtual async Task Update(CreateOrEditUserTimeSheetApproveDto input)
         {
            var userTimeSheetApprove = await _userTimeSheetApproveRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, userTimeSheetApprove);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _userTimeSheetApproveRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserTimeSheetApprovesToExcel(GetAllUserTimeSheetApprovesForExcelInput input)
         {
			
			var filteredUserTimeSheetApproves = _userTimeSheetApproveRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ProjectManagerFk)
						.Include( e => e.ProjectFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ApprovedUnits.Contains(input.Filter))
						.WhereIf(input.MinMonthFilter != null, e => e.Month >= input.MinMonthFilter)
						.WhereIf(input.MaxMonthFilter != null, e => e.Month <= input.MaxMonthFilter)
						.WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
						.WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedUnitsFilter),  e => e.ApprovedUnits == input.ApprovedUnitsFilter)
						.WhereIf(input.ProjectManagerApproveFilter > -1,  e => (input.ProjectManagerApproveFilter == 1 && e.ProjectManagerApprove) || (input.ProjectManagerApproveFilter == 0 && !e.ProjectManagerApprove) )
						.WhereIf(input.IsClosedFilter > -1,  e => (input.IsClosedFilter == 1 && e.IsClosed) || (input.IsClosedFilter == 0 && !e.IsClosed) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.ProjectManagerFk != null && e.ProjectManagerFk.Name == input.UserName2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameEnFilter), e => e.ProjectFk != null && e.ProjectFk.NameEn == input.ProjectNameEnFilter);

			var query = (from o in filteredUserTimeSheetApproves
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.ProjectManagerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_projectRepository.GetAll() on o.ProjectId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetUserTimeSheetApproveForViewDto() { 
							UserTimeSheetApprove = new UserTimeSheetApproveDto
							{
                                Month = o.Month,
                                Year = o.Year,
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                ApprovedUnits = o.ApprovedUnits,
                                ProjectManagerApprove = o.ProjectManagerApprove,
                                IsClosed = o.IsClosed,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName2 = s2 == null ? "" : s2.Name.ToString(),
                         	ProjectNameEn = s3 == null ? "" : s3.NameEn.ToString()
						 });


            var userTimeSheetApproveListDtos = await query.ToListAsync();

            return _userTimeSheetApprovesExcelExporter.ExportToFile(userTimeSheetApproveListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves)]
         public async Task<PagedResultDto<UserTimeSheetApproveUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserTimeSheetApproveUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserTimeSheetApproveUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserTimeSheetApproveUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_UserTimeSheetApproves)]
         public async Task<PagedResultDto<UserTimeSheetApproveProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_projectRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var projectList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserTimeSheetApproveProjectLookupTableDto>();
			foreach(var project in projectList){
				lookupTableDtoList.Add(new UserTimeSheetApproveProjectLookupTableDto
				{
					Id = project.Id,
					DisplayName = project.NameEn?.ToString()
				});
			}

            return new PagedResultDto<UserTimeSheetApproveProjectLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}