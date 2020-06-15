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
	[AbpAuthorize(AppPermissions.Pages_EmployeeWarnings)]
    public class EmployeeWarningsAppService : AttendanceAppServiceBase, IEmployeeWarningsAppService
    {
		 private readonly IRepository<EmployeeWarning> _employeeWarningRepository;
		 private readonly IEmployeeWarningsExcelExporter _employeeWarningsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<WarningType,int> _lookup_warningTypeRepository;
		 

		  public EmployeeWarningsAppService(IRepository<EmployeeWarning> employeeWarningRepository, IEmployeeWarningsExcelExporter employeeWarningsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<WarningType, int> lookup_warningTypeRepository) 
		  {
			_employeeWarningRepository = employeeWarningRepository;
			_employeeWarningsExcelExporter = employeeWarningsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_warningTypeRepository = lookup_warningTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeeWarningForViewDto>> GetAll(GetAllEmployeeWarningsInput input)
         {
			
			var filteredEmployeeWarnings = _employeeWarningRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.WarningTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WarningTypeNameArFilter), e => e.WarningTypeFk != null && e.WarningTypeFk.NameAr == input.WarningTypeNameArFilter);

			var pagedAndFilteredEmployeeWarnings = filteredEmployeeWarnings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeWarnings = from o in pagedAndFilteredEmployeeWarnings
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_warningTypeRepository.GetAll() on o.WarningTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeWarningForViewDto() {
							EmployeeWarning = new EmployeeWarningDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	WarningTypeNameAr = s2 == null ? "" : s2.NameAr.ToString()
						};

            var totalCount = await filteredEmployeeWarnings.CountAsync();

            return new PagedResultDto<GetEmployeeWarningForViewDto>(
                totalCount,
                await employeeWarnings.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeWarningForViewDto> GetEmployeeWarningForView(int id)
         {
            var employeeWarning = await _employeeWarningRepository.GetAsync(id);

            var output = new GetEmployeeWarningForViewDto { EmployeeWarning = ObjectMapper.Map<EmployeeWarningDto>(employeeWarning) };

		    if (output.EmployeeWarning.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeWarning.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeeWarning.WarningTypeId != null)
            {
                var _lookupWarningType = await _lookup_warningTypeRepository.FirstOrDefaultAsync((int)output.EmployeeWarning.WarningTypeId);
                output.WarningTypeNameAr = _lookupWarningType.NameAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeWarnings_Edit)]
		 public async Task<GetEmployeeWarningForEditOutput> GetEmployeeWarningForEdit(EntityDto input)
         {
            var employeeWarning = await _employeeWarningRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeWarningForEditOutput {EmployeeWarning = ObjectMapper.Map<CreateOrEditEmployeeWarningDto>(employeeWarning)};

		    if (output.EmployeeWarning.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeWarning.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeeWarning.WarningTypeId != null)
            {
                var _lookupWarningType = await _lookup_warningTypeRepository.FirstOrDefaultAsync((int)output.EmployeeWarning.WarningTypeId);
                output.WarningTypeNameAr = _lookupWarningType.NameAr.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeWarningDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeWarnings_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeWarningDto input)
         {
            var employeeWarning = ObjectMapper.Map<EmployeeWarning>(input);

			

            await _employeeWarningRepository.InsertAsync(employeeWarning);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeWarnings_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeWarningDto input)
         {
            var employeeWarning = await _employeeWarningRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeWarning);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeWarnings_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeWarningRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeWarningsToExcel(GetAllEmployeeWarningsForExcelInput input)
         {
			
			var filteredEmployeeWarnings = _employeeWarningRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.WarningTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WarningTypeNameArFilter), e => e.WarningTypeFk != null && e.WarningTypeFk.NameAr == input.WarningTypeNameArFilter);

			var query = (from o in filteredEmployeeWarnings
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_warningTypeRepository.GetAll() on o.WarningTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeWarningForViewDto() { 
							EmployeeWarning = new EmployeeWarningDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	WarningTypeNameAr = s2 == null ? "" : s2.NameAr.ToString()
						 });


            var employeeWarningListDtos = await query.ToListAsync();

            return _employeeWarningsExcelExporter.ExportToFile(employeeWarningListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeWarnings)]
         public async Task<PagedResultDto<EmployeeWarningUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeWarningUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new EmployeeWarningUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeeWarningUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_EmployeeWarnings)]
         public async Task<PagedResultDto<EmployeeWarningWarningTypeLookupTableDto>> GetAllWarningTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_warningTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var warningTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeWarningWarningTypeLookupTableDto>();
			foreach(var warningType in warningTypeList){
				lookupTableDtoList.Add(new EmployeeWarningWarningTypeLookupTableDto
				{
					Id = warningType.Id,
					DisplayName = warningType.NameAr?.ToString()
				});
			}

            return new PagedResultDto<EmployeeWarningWarningTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}