using Abp.Organizations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.GEC.Attendance.Setting.Exporting;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.Setting
{
	[AbpAuthorize(AppPermissions.Pages_Machines)]
    public class MachinesAppService : AttendanceAppServiceBase, IMachinesAppService
    {
		 private readonly IRepository<Machine> _machineRepository;
		 private readonly IMachinesExcelExporter _machinesExcelExporter;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 

		  public MachinesAppService(IRepository<Machine> machineRepository, IMachinesExcelExporter machinesExcelExporter , IRepository<OrganizationUnit, long> lookup_organizationUnitRepository) 
		  {
			_machineRepository = machineRepository;
			_machinesExcelExporter = machinesExcelExporter;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		
		  }

		 public async Task<PagedResultDto<GetMachineForViewDto>> GetAll(GetAllMachinesInput input)
         {
			
			var filteredMachines = _machineRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IpAddress.Contains(input.Filter) || e.SubNet.Contains(input.Filter) || e.Action.Contains(input.Filter) || e.CmdStatus.Contains(input.Filter) || e.Port.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IpAddressFilter),  e => e.IpAddress == input.IpAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubNetFilter),  e => e.SubNet == input.SubNetFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var pagedAndFilteredMachines = filteredMachines
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var machines = from o in pagedAndFilteredMachines
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMachineForViewDto() {
							Machine = new MachineDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                IpAddress = o.IpAddress,
                                SubNet = o.SubNet,
                                Status = o.Status,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
						};

            var totalCount = await filteredMachines.CountAsync();

            return new PagedResultDto<GetMachineForViewDto>(
                totalCount,
                await machines.ToListAsync()
            );
         }
		 
		 public async Task<GetMachineForViewDto> GetMachineForView(int id)
         {
            var machine = await _machineRepository.GetAsync(id);

            var output = new GetMachineForViewDto { Machine = ObjectMapper.Map<MachineDto>(machine) };

		    if (output.Machine.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Machine.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Machines_Edit)]
		 public async Task<GetMachineForEditOutput> GetMachineForEdit(EntityDto input)
         {
            var machine = await _machineRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMachineForEditOutput {Machine = ObjectMapper.Map<CreateOrEditMachineDto>(machine)};

		    if (output.Machine.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Machine.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMachineDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Machines_Create)]
		 protected virtual async Task Create(CreateOrEditMachineDto input)
         {
            var machine = ObjectMapper.Map<Machine>(input);

			

            await _machineRepository.InsertAsync(machine);
         }

		 [AbpAuthorize(AppPermissions.Pages_Machines_Edit)]
		 protected virtual async Task Update(CreateOrEditMachineDto input)
         {
            var machine = await _machineRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, machine);
         }

		 [AbpAuthorize(AppPermissions.Pages_Machines_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _machineRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMachinesToExcel(GetAllMachinesForExcelInput input)
         {
			
			var filteredMachines = _machineRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IpAddress.Contains(input.Filter) || e.SubNet.Contains(input.Filter) || e.Action.Contains(input.Filter) || e.CmdStatus.Contains(input.Filter) || e.Port.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IpAddressFilter),  e => e.IpAddress == input.IpAddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubNetFilter),  e => e.SubNet == input.SubNetFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var query = (from o in filteredMachines
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetMachineForViewDto() { 
							Machine = new MachineDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                IpAddress = o.IpAddress,
                                SubNet = o.SubNet,
                                Status = o.Status,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
						 });


            var machineListDtos = await query.ToListAsync();

            return _machinesExcelExporter.ExportToFile(machineListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Machines)]
         public async Task<PagedResultDto<MachineOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<MachineOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new MachineOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<MachineOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}