

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
	[AbpAuthorize(AppPermissions.Pages_Locations)]
    public class LocationsAppService : AttendanceAppServiceBase, ILocationsAppService
    {
		 private readonly IRepository<Location> _locationRepository;
		 private readonly ILocationsExcelExporter _locationsExcelExporter;
		 

		  public LocationsAppService(IRepository<Location> locationRepository, ILocationsExcelExporter locationsExcelExporter ) 
		  {
			_locationRepository = locationRepository;
			_locationsExcelExporter = locationsExcelExporter;
			
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
            var location = await _locationRepository.GetAllIncluding(a => a.LocationCredentials).FirstOrDefaultAsync(x => x.Id == input.Id);
           
		    var output = new GetLocationForEditOutput {Location = ObjectMapper.Map<CreateOrEditLocationDto>(location)};
			
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


            location.LocationCredentials = new List<LocationCredential>();

            if (input.LocationCredentials.Count > 0)
            {
                foreach (var item in input.LocationCredentials)
                {
                    location.LocationCredentials.Add(ObjectMapper.Map<LocationCredential>(item));
                }

            }


            await _locationRepository.InsertAsync(location);
         }

		 [AbpAuthorize(AppPermissions.Pages_Locations_Edit)]
		 protected virtual async Task Update(CreateOrEditLocationDto input)
         {
            var location = await _locationRepository.GetAllIncluding(m => m.LocationCredentials).FirstOrDefaultAsync(x => x.Id == (int)input.Id);

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