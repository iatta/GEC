using Pixel.Attendance.Authorization.Users;
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
	[AbpAuthorize(AppPermissions.Pages_UserDelegations)]
    public class UserDelegationsAppService : AttendanceAppServiceBase, IUserDelegationsAppService
    {
		 private readonly IRepository<UserDelegation> _userDelegationRepository;
		 private readonly IUserDelegationsExcelExporter _userDelegationsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public UserDelegationsAppService(IRepository<UserDelegation> userDelegationRepository, IUserDelegationsExcelExporter userDelegationsExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_userDelegationRepository = userDelegationRepository;
			_userDelegationsExcelExporter = userDelegationsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserDelegationForViewDto>> GetAll(GetAllUserDelegationsInput input)
         {
			
			var filteredUserDelegations = _userDelegationRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DelegatedUserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DelegatedUserFk != null && e.DelegatedUserFk.Name == input.UserName2Filter);

			var pagedAndFilteredUserDelegations = filteredUserDelegations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userDelegations = from o in pagedAndFilteredUserDelegations
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.DelegatedUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDelegationForViewDto() {
							UserDelegation = new UserDelegationDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName2 = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredUserDelegations.CountAsync();

            return new PagedResultDto<GetUserDelegationForViewDto>(
                totalCount,
                await userDelegations.ToListAsync()
            );
         }
		 
		 public async Task<GetUserDelegationForViewDto> GetUserDelegationForView(int id)
         {
            var userDelegation = await _userDelegationRepository.GetAsync(id);

            var output = new GetUserDelegationForViewDto { UserDelegation = ObjectMapper.Map<UserDelegationDto>(userDelegation) };

		    if (output.UserDelegation.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDelegation.UserId);
                output.UserName = _lookupUser?.Name.ToString();
            }

		    if (output.UserDelegation.DelegatedUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDelegation.DelegatedUserId);
                output.UserName2 = _lookupUser?.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UserDelegations_Edit)]
		 public async Task<GetUserDelegationForEditOutput> GetUserDelegationForEdit(EntityDto input)
         {
            var userDelegation = await _userDelegationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserDelegationForEditOutput {UserDelegation = ObjectMapper.Map<CreateOrEditUserDelegationDto>(userDelegation)};

		    if (output.UserDelegation.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDelegation.UserId);
                output.UserName = _lookupUser?.Name.ToString();
            }

		    if (output.UserDelegation.DelegatedUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDelegation.DelegatedUserId);
                output.UserName2 = _lookupUser?.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserDelegationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDelegations_Create)]
		 protected virtual async Task Create(CreateOrEditUserDelegationDto input)
         {
            var userDelegation = ObjectMapper.Map<UserDelegation>(input);

			

            await _userDelegationRepository.InsertAsync(userDelegation);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDelegations_Edit)]
		 protected virtual async Task Update(CreateOrEditUserDelegationDto input)
         {
            var userDelegation = await _userDelegationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, userDelegation);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDelegations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _userDelegationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserDelegationsToExcel(GetAllUserDelegationsForExcelInput input)
         {
			
			var filteredUserDelegations = _userDelegationRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DelegatedUserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DelegatedUserFk != null && e.DelegatedUserFk.Name == input.UserName2Filter);

			var query = (from o in filteredUserDelegations
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.DelegatedUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDelegationForViewDto() { 
							UserDelegation = new UserDelegationDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName2 = s2 == null ? "" : s2.Name.ToString()
						 });


            var userDelegationListDtos = await query.ToListAsync();

            return _userDelegationsExcelExporter.ExportToFile(userDelegationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UserDelegations)]
         public async Task<PagedResultDto<UserDelegationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDelegationUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserDelegationUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserDelegationUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}