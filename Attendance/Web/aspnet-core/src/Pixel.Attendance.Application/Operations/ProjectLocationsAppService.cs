using Pixel.Attendance.Operations;
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
	[AbpAuthorize(AppPermissions.Pages_ProjectLocations)]
    public class ProjectLocationsAppService : AttendanceAppServiceBase, IProjectLocationsAppService
    {
		 private readonly IRepository<ProjectLocation> _projectLocationRepository;
		 private readonly IProjectLocationsExcelExporter _projectLocationsExcelExporter;
		 private readonly IRepository<Project,int> _lookup_projectRepository;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 

		  public ProjectLocationsAppService(IRepository<ProjectLocation> projectLocationRepository, IProjectLocationsExcelExporter projectLocationsExcelExporter , IRepository<Project, int> lookup_projectRepository, IRepository<Location, int> lookup_locationRepository) 
		  {
			_projectLocationRepository = projectLocationRepository;
			_projectLocationsExcelExporter = projectLocationsExcelExporter;
			_lookup_projectRepository = lookup_projectRepository;
		_lookup_locationRepository = lookup_locationRepository;
		
		  }

		 public async Task<PagedResultDto<GetProjectLocationForViewDto>> GetAll(GetAllProjectLocationsInput input)
         {
			
			var filteredProjectLocations = _projectLocationRepository.GetAll()
						.Include( e => e.ProjectFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameEnFilter), e => e.ProjectFk != null && e.ProjectFk.NameEn == input.ProjectNameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var pagedAndFilteredProjectLocations = filteredProjectLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projectLocations = from o in pagedAndFilteredProjectLocations
                         join o1 in _lookup_projectRepository.GetAll() on o.ProjectId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetProjectLocationForViewDto() {
							ProjectLocation = new ProjectLocationDto
							{
                                Id = o.Id
							},
                         	ProjectNameEn = s1 == null ? "" : s1.NameEn.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString()
						};

            var totalCount = await filteredProjectLocations.CountAsync();

            return new PagedResultDto<GetProjectLocationForViewDto>(
                totalCount,
                await projectLocations.ToListAsync()
            );
         }
		 
		 public async Task<GetProjectLocationForViewDto> GetProjectLocationForView(int id)
         {
            var projectLocation = await _projectLocationRepository.GetAsync(id);

            var output = new GetProjectLocationForViewDto { ProjectLocation = ObjectMapper.Map<ProjectLocationDto>(projectLocation) };

		    if (output.ProjectLocation.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.ProjectLocation.ProjectId);
                output.ProjectNameEn = _lookupProject.NameEn.ToString();
            }

		    if (output.ProjectLocation.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.ProjectLocation.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ProjectLocations_Edit)]
		 public async Task<GetProjectLocationForEditOutput> GetProjectLocationForEdit(EntityDto input)
         {
            var projectLocation = await _projectLocationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProjectLocationForEditOutput {ProjectLocation = ObjectMapper.Map<CreateOrEditProjectLocationDto>(projectLocation)};

		    if (output.ProjectLocation.ProjectId != null)
            {
                var _lookupProject = await _lookup_projectRepository.FirstOrDefaultAsync((int)output.ProjectLocation.ProjectId);
                output.ProjectNameEn = _lookupProject.NameEn.ToString();
            }

		    if (output.ProjectLocation.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.ProjectLocation.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProjectLocationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectLocations_Create)]
		 protected virtual async Task Create(CreateOrEditProjectLocationDto input)
         {
            var projectLocation = ObjectMapper.Map<ProjectLocation>(input);

			

            await _projectLocationRepository.InsertAsync(projectLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectLocations_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectLocationDto input)
         {
            var projectLocation = await _projectLocationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, projectLocation);
         }

		 [AbpAuthorize(AppPermissions.Pages_ProjectLocations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _projectLocationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProjectLocationsToExcel(GetAllProjectLocationsForExcelInput input)
         {
			
			var filteredProjectLocations = _projectLocationRepository.GetAll()
						.Include( e => e.ProjectFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameEnFilter), e => e.ProjectFk != null && e.ProjectFk.NameEn == input.ProjectNameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var query = (from o in filteredProjectLocations
                         join o1 in _lookup_projectRepository.GetAll() on o.ProjectId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetProjectLocationForViewDto() { 
							ProjectLocation = new ProjectLocationDto
							{
                                Id = o.Id
							},
                         	ProjectNameEn = s1 == null ? "" : s1.NameEn.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString()
						 });


            var projectLocationListDtos = await query.ToListAsync();

            return _projectLocationsExcelExporter.ExportToFile(projectLocationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ProjectLocations)]
         public async Task<PagedResultDto<ProjectLocationProjectLookupTableDto>> GetAllProjectForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_projectRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var projectList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectLocationProjectLookupTableDto>();
			foreach(var project in projectList){
				lookupTableDtoList.Add(new ProjectLocationProjectLookupTableDto
				{
					Id = project.Id,
					DisplayName = project.NameEn?.ToString()
				});
			}

            return new PagedResultDto<ProjectLocationProjectLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ProjectLocations)]
         public async Task<PagedResultDto<ProjectLocationLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_locationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TitleEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var locationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectLocationLocationLookupTableDto>();
			foreach(var location in locationList){
				lookupTableDtoList.Add(new ProjectLocationLocationLookupTableDto
				{
					Id = location.Id,
					DisplayName = location.TitleEn?.ToString()
				});
			}

            return new PagedResultDto<ProjectLocationLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}