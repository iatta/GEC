using Abp.Organizations;
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
	[AbpAuthorize(AppPermissions.Pages_OrganizationLocations)]
    public class OrganizationLocationsAppService : AttendanceAppServiceBase, IOrganizationLocationsAppService
    {
		 private readonly IRepository<OrganizationLocation> _organizationLocationRepository;
		 private readonly IOrganizationLocationsExcelExporter _organizationLocationsExcelExporter;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 

		  public OrganizationLocationsAppService(IRepository<OrganizationLocation> organizationLocationRepository, IOrganizationLocationsExcelExporter organizationLocationsExcelExporter , IRepository<OrganizationUnit, long> lookup_organizationUnitRepository, IRepository<Location, int> lookup_locationRepository) 
		  {
			_organizationLocationRepository = organizationLocationRepository;
			_organizationLocationsExcelExporter = organizationLocationsExcelExporter;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		_lookup_locationRepository = lookup_locationRepository;
		
		  }

		 public async Task<PagedResultDto<GetOrganizationLocationForViewDto>> GetAll(GetAllOrganizationLocationsInput input)
         {
			
			var filteredOrganizationLocations = _organizationLocationRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var pagedAndFilteredOrganizationLocations = filteredOrganizationLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var organizationLocations = from o in pagedAndFilteredOrganizationLocations
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetOrganizationLocationForViewDto() {
							OrganizationLocation = new OrganizationLocationDto
							{
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString()
						};

            var totalCount = await filteredOrganizationLocations.CountAsync();

            return new PagedResultDto<GetOrganizationLocationForViewDto>(
                totalCount,
                await organizationLocations.ToListAsync()
            );
         }
		 
		 public async Task<GetOrganizationLocationForViewDto> GetOrganizationLocationForView(int id)
         {
            var organizationLocation = await _organizationLocationRepository.GetAsync(id);

            var output = new GetOrganizationLocationForViewDto { OrganizationLocation = ObjectMapper.Map<OrganizationLocationDto>(organizationLocation) };

		    if (output.OrganizationLocation.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.OrganizationLocation.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.OrganizationLocation.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.OrganizationLocation.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_OrganizationLocations_Edit)]
		 public async Task<GetOrganizationLocationForEditOutput> GetOrganizationLocationForEdit(EntityDto input)
         {
            var organizationLocation = await _organizationLocationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetOrganizationLocationForEditOutput {OrganizationLocation = ObjectMapper.Map<CreateOrEditOrganizationLocationDto>(organizationLocation)};

		    if (output.OrganizationLocation.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.OrganizationLocation.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.OrganizationLocation.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.OrganizationLocation.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditOrganizationLocationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationLocations_Create)]
		 protected virtual async Task Create(CreateOrEditOrganizationLocationDto input)
         {
            var organizationLocation = ObjectMapper.Map<OrganizationLocation>(input);

			

            await _organizationLocationRepository.InsertAsync(organizationLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationLocations_Edit)]
		 protected virtual async Task Update(CreateOrEditOrganizationLocationDto input)
         {
            var organizationLocation = await _organizationLocationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, organizationLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationLocations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _organizationLocationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetOrganizationLocationsToExcel(GetAllOrganizationLocationsForExcelInput input)
         {
			
			var filteredOrganizationLocations = _organizationLocationRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var query = (from o in filteredOrganizationLocations
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetOrganizationLocationForViewDto() { 
							OrganizationLocation = new OrganizationLocationDto
							{
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString()
						 });


            var organizationLocationListDtos = await query.ToListAsync();

            return _organizationLocationsExcelExporter.ExportToFile(organizationLocationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_OrganizationLocations)]
         public async Task<PagedResultDto<OrganizationLocationOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<OrganizationLocationOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new OrganizationLocationOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<OrganizationLocationOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_OrganizationLocations)]
         public async Task<PagedResultDto<OrganizationLocationLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_locationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TitleEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var locationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<OrganizationLocationLocationLookupTableDto>();
			foreach(var location in locationList){
				lookupTableDtoList.Add(new OrganizationLocationLocationLookupTableDto
				{
					Id = location.Id,
					DisplayName = location.TitleEn?.ToString()
				});
			}

            return new PagedResultDto<OrganizationLocationLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}