

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
	[AbpAuthorize(AppPermissions.Pages_Locations)]
    public class LocationsAppService : AttendanceAppServiceBase, ILocationsAppService
    {
		 private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Machine> _machineRepository;
        private readonly ILocationsExcelExporter _locationsExcelExporter;
		 

		  public LocationsAppService(IRepository<Machine> machineRepository,IRepository<Location> locationRepository, ILocationsExcelExporter locationsExcelExporter ) 
		  {
			_locationRepository = locationRepository;
			_locationsExcelExporter = locationsExcelExporter;
            _machineRepository = machineRepository;


          }

		 public async Task<PagedResultDto<GetLocationForViewDto>> GetAll(GetAllLocationsInput input)
         {
			
			var filteredLocations = _locationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.TitleAr.Contains(input.Filter) || e.TitleEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleArFilter),  e => e.TitleAr == input.TitleArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleEnFilter),  e => e.TitleEn == input.TitleEnFilter);

			var pagedAndFilteredLocations = filteredLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var locations = from o in pagedAndFilteredLocations
                         select new GetLocationForViewDto() {
							Location = new LocationDto
							{
                                TitleAr = o.TitleAr,
                                TitleEn = o.TitleEn,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLocations.CountAsync();

            return new PagedResultDto<GetLocationForViewDto>(
                totalCount,
                await locations.ToListAsync()
            );
         }
		 
		 public async Task<GetLocationForViewDto> GetLocationForView(int id)
         {
            var location = await _locationRepository.GetAsync(id);

            var output = new GetLocationForViewDto { Location = ObjectMapper.Map<LocationDto>(location) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Locations_Edit)]
		 public async Task<GetLocationForEditOutput> GetLocationForEdit(EntityDto input)
         {
            var location = await _locationRepository.GetAllIncluding(a => a.LocationCredentials,x => x.Machines).FirstOrDefaultAsync(x => x.Id == input.Id);
           
		    var output = new GetLocationForEditOutput {Location = ObjectMapper.Map<CreateOrEditLocationDto>(location)};
            foreach (var locationMachine in output.Location.Machines)
            {
                locationMachine.MachineName = _machineRepository.FirstOrDefault(x => x.Id == locationMachine.MachineId).NameEn;
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLocationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Locations_Create)]
		 protected virtual async Task Create(CreateOrEditLocationDto input)
         {
            var location = ObjectMapper.Map<Location>(input);

            location.Machines = new List<LocationMachine>();
            //location.LocationCredentials = new List<LocationCredential>();

            //if (input.LocationCredentials.Count > 0)
            //{
            //    foreach (var item in input.LocationCredentials)
            //    {
            //        location.LocationCredentials.Add(ObjectMapper.Map<LocationCredential>(item));
            //    }

            //}

            if (input.Machines.Count > 0)
            {
                foreach (var item in input.Machines)
                {
                    location.Machines.Add(ObjectMapper.Map<LocationMachine>(item));
                }

            }


            await _locationRepository.InsertAsync(location);
         }

		 [AbpAuthorize(AppPermissions.Pages_Locations_Edit)]
		 protected virtual async Task Update(CreateOrEditLocationDto input)
         {
            var location = await _locationRepository.GetAllIncluding(m => m.LocationCredentials,x => x.Machines).FirstOrDefaultAsync(x => x.Id == (int)input.Id);

            var oldLocationMachines = new HashSet<LocationMachine>(location.Machines.ToList());
            var newLocationMachines = new HashSet<LocationMachineDto>(input.Machines.ToList());

            var oldLocationCredentials = new HashSet<LocationCredential>(location.LocationCredentials.ToList());
            var newLocationCredentials = new HashSet<LocationCredentialDto>(input.LocationCredentials.ToList());

            foreach (var detail in oldLocationCredentials)
            {
                if (!newLocationCredentials.Any(x => x.Id == detail.Id))
                {
                    location.LocationCredentials.Remove(detail);
                }
                else
                {
                    var inputDetail = newLocationCredentials.Where(x => x.Id == detail.Id).FirstOrDefault();
                    detail.Longitude = inputDetail.Longitude;
                    detail.Latitude = inputDetail.Latitude;
                }

            }


            foreach (var item in newLocationCredentials)
            {
                if (item.Id == 0)
                {
                    location.LocationCredentials.Add(ObjectMapper.Map<LocationCredential>(item));
                }
            }

            foreach (var detail in oldLocationMachines)
            {
                if (!newLocationMachines.Any(x => x.Id == detail.Id))
                {
                    location.Machines.Remove(detail);
                }
                else
                {
                    var inputDetail = newLocationMachines.Where(x => x.Id == detail.Id).FirstOrDefault();
                }

            }

            foreach (var item in newLocationMachines)
            {
                if (item.Id == 0)
                {
                    location.Machines.Add(ObjectMapper.Map<LocationMachine>(item));
                }
            }

            location.TitleAr = input.TitleAr;
            location.TitleEn = input.TitleEn;


            await _locationRepository.UpdateAsync(location);
        }

		 [AbpAuthorize(AppPermissions.Pages_Locations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _locationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLocationsToExcel(GetAllLocationsForExcelInput input)
         {
			
			var filteredLocations = _locationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.TitleAr.Contains(input.Filter) || e.TitleEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleArFilter),  e => e.TitleAr == input.TitleArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleEnFilter),  e => e.TitleEn == input.TitleEnFilter);

			var query = (from o in filteredLocations
                         select new GetLocationForViewDto() { 
							Location = new LocationDto
							{
                                TitleAr = o.TitleAr,
                                TitleEn = o.TitleEn,
                                Id = o.Id
							}
						 });


            var locationListDtos = await query.ToListAsync();

            return _locationsExcelExporter.ExportToFile(locationListDtos);
         }


    }
}