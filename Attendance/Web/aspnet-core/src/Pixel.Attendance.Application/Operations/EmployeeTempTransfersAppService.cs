using Abp.Organizations;
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
	[AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers)]
    public class EmployeeTempTransfersAppService : AttendanceAppServiceBase, IEmployeeTempTransfersAppService
    {
		 private readonly IRepository<EmployeeTempTransfer> _employeeTempTransferRepository;
		 private readonly IEmployeeTempTransfersExcelExporter _employeeTempTransfersExcelExporter;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public EmployeeTempTransfersAppService(IRepository<EmployeeTempTransfer> employeeTempTransferRepository, IEmployeeTempTransfersExcelExporter employeeTempTransfersExcelExporter , IRepository<OrganizationUnit, long> lookup_organizationUnitRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_employeeTempTransferRepository = employeeTempTransferRepository;
			_employeeTempTransfersExcelExporter = employeeTempTransfersExcelExporter;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeeTempTransferForViewDto>> GetAll(GetAllEmployeeTempTransfersInput input)
         {
			
			var filteredEmployeeTempTransfers = _employeeTempTransferRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredEmployeeTempTransfers = filteredEmployeeTempTransfers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeTempTransfers = from o in pagedAndFilteredEmployeeTempTransfers
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeTempTransferForViewDto() {
							EmployeeTempTransfer = new EmployeeTempTransferDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredEmployeeTempTransfers.CountAsync();

            return new PagedResultDto<GetEmployeeTempTransferForViewDto>(
                totalCount,
                await employeeTempTransfers.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeTempTransferForViewDto> GetEmployeeTempTransferForView(int id)
         {
            var employeeTempTransfer = await _employeeTempTransferRepository.GetAsync(id);

            var output = new GetEmployeeTempTransferForViewDto { EmployeeTempTransfer = ObjectMapper.Map<EmployeeTempTransferDto>(employeeTempTransfer) };

		    if (output.EmployeeTempTransfer.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.EmployeeTempTransfer.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.EmployeeTempTransfer.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeTempTransfer.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers_Edit)]
		 public async Task<GetEmployeeTempTransferForEditOutput> GetEmployeeTempTransferForEdit(EntityDto input)
         {
            var employeeTempTransfer = await _employeeTempTransferRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeTempTransferForEditOutput {EmployeeTempTransfer = ObjectMapper.Map<CreateOrEditEmployeeTempTransferDto>(employeeTempTransfer)};

		    if (output.EmployeeTempTransfer.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.EmployeeTempTransfer.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.EmployeeTempTransfer.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeTempTransfer.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeTempTransferDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeTempTransferDto input)
         {
            var employeeTempTransfer = ObjectMapper.Map<EmployeeTempTransfer>(input);

			

            await _employeeTempTransferRepository.InsertAsync(employeeTempTransfer);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeTempTransferDto input)
         {
            var employeeTempTransfer = await _employeeTempTransferRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeTempTransfer);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeTempTransferRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeTempTransfersToExcel(GetAllEmployeeTempTransfersForExcelInput input)
         {
			
			var filteredEmployeeTempTransfers = _employeeTempTransferRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredEmployeeTempTransfers
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeTempTransferForViewDto() { 
							EmployeeTempTransfer = new EmployeeTempTransferDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						 });


            var employeeTempTransferListDtos = await query.ToListAsync();

            return _employeeTempTransfersExcelExporter.ExportToFile(employeeTempTransferListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers)]
         public async Task<PagedResultDto<EmployeeTempTransferOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeTempTransferOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new EmployeeTempTransferOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<EmployeeTempTransferOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_EmployeeTempTransfers)]
         public async Task<PagedResultDto<EmployeeTempTransferUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeTempTransferUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new EmployeeTempTransferUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeeTempTransferUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}