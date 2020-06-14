using GEC.Attendance.Setting;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using GEC.Attendance.Setting.Exporting;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GEC.Attendance.Setting
{
	[AbpAuthorize(AppPermissions.Pages_LocationCredentials)]
    public class LocationCredentialsAppService : AttendanceAppServiceBase, ILocationCredentialsAppService
    {
		 private readonly IRepository<LocationCredential> _locationCredentialRepository;
		 private readonly ILocationCredentialsExcelExporter _locationCredentialsExcelExporter;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 

		  public LocationCredentialsAppService(IRepository<LocationCredential> locationCredentialRepository, ILocationCredentialsExcelExporter locationCredentialsExcelExporter , IRepository<Location, int> lookup_locationRepository) 
		  {
			_locationCredentialRepository = locationCredentialRepository;
			_locationCredentialsExcelExporter = locationCredentialsExcelExporter;
			_lookup_locationRepository = lookup_locationRepository;
		
		  }

		 public async Task<PagedResultDto<GetLocationCredentialForViewDto>> GetAll(GetAllLocationCredentialsInput input)
         {
			
			var filteredLocationCredentials = _locationCredentialRepository.GetAll()
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
						.WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
						.WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
						.WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationDescriptionArFilter), e => e.LocationFk != null && e.LocationFk.TitleAr == input.LocationDescriptionArFilter);

			var pagedAndFilteredLocationCredentials = filteredLocationCredentials
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var locationCredentials = from o in pagedAndFilteredLocationCredentials
                         join o1 in _lookup_locationRepository.GetAll() on o.LocationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetLocationCredentialForViewDto() {
							LocationCredential = new LocationCredentialDto
							{
                                Longitude = o.Longitude,
                                Latitude = o.Latitude,
                                Id = o.Id
							},
                         	LocationDescriptionAr = s1 == null ? "" : s1.TitleAr.ToString()
						};

            var totalCount = await filteredLocationCredentials.CountAsync();

            return new PagedResultDto<GetLocationCredentialForViewDto>(
                totalCount,
                await locationCredentials.ToListAsync()
            );
         }
		 
		 public async Task<GetLocationCredentialForViewDto> GetLocationCredentialForView(int id)
         {
            var locationCredential = await _locationCredentialRepository.GetAsync(id);

            var output = new GetLocationCredentialForViewDto { LocationCredential = ObjectMapper.Map<LocationCredentialDto>(locationCredential) };

		    if (output.LocationCredential.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.LocationCredential.LocationId);
                output.LocationDescriptionAr = _lookupLocation.TitleAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LocationCredentials_Edit)]
		 public async Task<GetLocationCredentialForEditOutput> GetLocationCredentialForEdit(EntityDto input)
         {
            var locationCredential = await _locationCredentialRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLocationCredentialForEditOutput {LocationCredential = ObjectMapper.Map<CreateOrEditLocationCredentialDto>(locationCredential)};

		    if (output.LocationCredential.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.LocationCredential.LocationId);
                output.LocationDescriptionAr = _lookupLocation.TitleAr.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLocationCredentialDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationCredentials_Create)]
		 protected virtual async Task Create(CreateOrEditLocationCredentialDto input)
         {
            var locationCredential = ObjectMapper.Map<LocationCredential>(input);

			

            await _locationCredentialRepository.InsertAsync(locationCredential);
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationCredentials_Edit)]
		 protected virtual async Task Update(CreateOrEditLocationCredentialDto input)
         {
            var locationCredential = await _locationCredentialRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, locationCredential);
         }

		 [AbpAuthorize(AppPermissions.Pages_LocationCredentials_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _locationCredentialRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLocationCredentialsToExcel(GetAllLocationCredentialsForExcelInput input)
         {
			
			var filteredLocationCredentials = _locationCredentialRepository.GetAll()
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
						.WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
						.WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
						.WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationDescriptionArFilter), e => e.LocationFk != null && e.LocationFk.TitleAr == input.LocationDescriptionArFilter);

			var query = (from o in filteredLocationCredentials
                         join o1 in _lookup_locationRepository.GetAll() on o.LocationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetLocationCredentialForViewDto() { 
							LocationCredential = new LocationCredentialDto
							{
                                Longitude = o.Longitude,
                                Latitude = o.Latitude,
                                Id = o.Id
							},
                         	LocationDescriptionAr = s1 == null ? "" : s1.TitleAr.ToString()
						 });


            var locationCredentialListDtos = await query.ToListAsync();

            return _locationCredentialsExcelExporter.ExportToFile(locationCredentialListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_LocationCredentials)]
         public async Task<PagedResultDto<LocationCredentialLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_locationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TitleAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var locationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LocationCredentialLocationLookupTableDto>();
			foreach(var location in locationList){
				lookupTableDtoList.Add(new LocationCredentialLocationLookupTableDto
				{
					Id = location.Id,
					DisplayName = location.TitleAr?.ToString()
				});
			}

            return new PagedResultDto<LocationCredentialLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}