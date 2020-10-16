using Pixel.Attendance.Setting;
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
	[AbpAuthorize(AppPermissions.Pages_LocationMachines)]
    public class LocationMachinesAppService : AttendanceAppServiceBase, ILocationMachinesAppService
    {
		 private readonly IRepository<LocationMachine> _locationMachineRepository;
		 private readonly ILocationMachinesExcelExporter _locationMachinesExcelExporter;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 private readonly IRepository<Machine,int> _lookup_machineRepository;
		 

		  public LocationMachinesAppService(IRepository<LocationMachine> locationMachineRepository, ILocationMachinesExcelExporter locationMachinesExcelExporter , IRepository<Location, int> lookup_locationRepository, IRepository<Machine, int> lookup_machineRepository) 
		  {
			_locationMachineRepository = locationMachineRepository;
			_locationMachinesExcelExporter = locationMachinesExcelExporter;
			_lookup_locationRepository = lookup_locationRepository;
		_lookup_machineRepository = lookup_machineRepository;
		
		  }

		 public async Task<PagedResultDto<GetLocationMachineForViewDto>> GetAll(GetAllLocationMachinesInput input)
         {
			
			var filteredLocationMachines = _locationMachineRepository.GetAll()
						.Include( e => e.LocationFk)
						.Include( e => e.MachineFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleArFilter), e => e.LocationFk != null && e.LocationFk.TitleAr == input.LocationTitleArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MachineNameEnFilter), e => e.MachineFk != null && e.MachineFk.NameEn == input.MachineNameEnFilter);

			var pagedAndFilteredLocationMachines = filteredLocationMachines
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var locationMachines = from o in pagedAndFilteredLocationMachines
                         join o1 in _lookup_locationRepository.GetAll() on o.LocationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_machineRepository.GetAll() on o.MachineId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetLocationMachineForViewDto() {
							LocationMachine = new LocationMachineDto
							{
                                Id = o.Id
							},
                         	LocationTitleAr = s1 == null ? "" : s1.TitleAr.ToString(),
                         	MachineNameEn = s2 == null ? "" : s2.NameEn.ToString()
						};

            var totalCount = await filteredLocationMachines.CountAsync();

            return new PagedResultDto<GetLocationMachineForViewDto>(
                totalCount,
                await locationMachines.ToListAsync()
            );
         }
		 
		 public async Task<GetLocationMachineForViewDto> GetLocationMachineForView(int id)
         {
            var locationMachine = await _locationMachineRepository.GetAsync(id);

            var output = new GetLocationMachineForViewDto { LocationMachine = ObjectMapper.Map<LocationMachineDto>(locationMachine) };

		    if (output.LocationMachine.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.LocationMachine.LocationId);
                output.LocationTitleAr = _lookupLocation.TitleAr.ToString();
            }

		    if (output.LocationMachine.MachineId != null)
            {
                var _lookupMachine = await _lookup_machineRepository.FirstOrDefaultAsync((int)output.LocationMachine.MachineId);
                output.MachineNameEn = _lookupMachine.NameEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LocationMachines_Edit)]
		 public async Task<GetLocationMachineForEditOutput> GetLocationMachineForEdit(EntityDto input)
         {
            var locationMachine = await _locationMachineRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLocationMachineForEditOutput {LocationMachine = ObjectMapper.Map<CreateOrEditLocationMachineDto>(locationMachine)};

		    if (output.LocationMachine.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.LocationMachine.LocationId);
                output.LocationTitleAr = _lookupLocation.TitleAr.ToString();
            }

		    if (output.LocationMachine.MachineId != null)
            {
                var _lookupMachine = await _lookup_machineRepository.FirstOrDefaultAsync((int)output.LocationMachine.MachineId);
                output.MachineNameEn = _lookupMachine.NameEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLocationMachineDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationMachines_Create)]
		 protected virtual async Task Create(CreateOrEditLocationMachineDto input)
         {
            var locationMachine = ObjectMapper.Map<LocationMachine>(input);

			

            await _locationMachineRepository.InsertAsync(locationMachine);
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationMachines_Edit)]
		 protected virtual async Task Update(CreateOrEditLocationMachineDto input)
         {
            var locationMachine = await _locationMachineRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, locationMachine);
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationMachines_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _locationMachineRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLocationMachinesToExcel(GetAllLocationMachinesForExcelInput input)
         {
			
			var filteredLocationMachines = _locationMachineRepository.GetAll()
						.Include( e => e.LocationFk)
						.Include( e => e.MachineFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleArFilter), e => e.LocationFk != null && e.LocationFk.TitleAr == input.LocationTitleArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MachineNameEnFilter), e => e.MachineFk != null && e.MachineFk.NameEn == input.MachineNameEnFilter);

			var query = (from o in filteredLocationMachines
                         join o1 in _lookup_locationRepository.GetAll() on o.LocationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_machineRepository.GetAll() on o.MachineId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetLocationMachineForViewDto() { 
							LocationMachine = new LocationMachineDto
							{
                                Id = o.Id
							},
                         	LocationTitleAr = s1 == null ? "" : s1.TitleAr.ToString(),
                         	MachineNameEn = s2 == null ? "" : s2.NameEn.ToString()
						 });


            var locationMachineListDtos = await query.ToListAsync();

            return _locationMachinesExcelExporter.ExportToFile(locationMachineListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_LocationMachines)]
         public async Task<PagedResultDto<LocationMachineLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_locationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TitleAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var locationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LocationMachineLocationLookupTableDto>();
			foreach(var location in locationList){
				lookupTableDtoList.Add(new LocationMachineLocationLookupTableDto
				{
					Id = location.Id,
					DisplayName = location.TitleAr?.ToString()
				});
			}

            return new PagedResultDto<LocationMachineLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LocationMachines)]
         public async Task<PagedResultDto<LocationMachineMachineLookupTableDto>> GetAllMachineForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_machineRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var machineList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LocationMachineMachineLookupTableDto>();
			foreach(var machine in machineList){
				lookupTableDtoList.Add(new LocationMachineMachineLookupTableDto
				{
					Id = machine.Id,
					DisplayName = machine.NameEn?.ToString()
				});
			}

            return new PagedResultDto<LocationMachineMachineLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}