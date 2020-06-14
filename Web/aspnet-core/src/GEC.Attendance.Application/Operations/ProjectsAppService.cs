using GEC.Attendance.Authorization.Users;
using Abp.Organizations;
using GEC.Attendance.Setting;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using GEC.Attendance.Operations.Exporting;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GEC.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_Projects)]
    public class ProjectsAppService : AttendanceAppServiceBase, IProjectsAppService
    {
		 private readonly IRepository<Project> _projectRepository;
		 private readonly IProjectsExcelExporter _projectsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 

		  public ProjectsAppService(IRepository<Project> projectRepository, IProjectsExcelExporter projectsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<OrganizationUnit, long> lookup_organizationUnitRepository, IRepository<Location, int> lookup_locationRepository) 
		  {
			_projectRepository = projectRepository;
			_projectsExcelExporter = projectsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		_lookup_locationRepository = lookup_locationRepository;
		
		  }

		 public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
         {
			
			var filteredProjects = _projectRepository.GetAll()
						.Include( e => e.ManagerFk)
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ManagerFk != null && e.ManagerFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projects = from o in pagedAndFilteredProjects
                         join o1 in _lookup_userRepository.GetAll() on o.ManagerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_locationRepository.GetAll() on o.LocationId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() {
							Project = new ProjectDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	OrganizationUnitDisplayName = s2 == null ? "" : s2.DisplayName.ToString(),
                         	LocationTitleEn = s3 == null ? "" : s3.TitleEn.ToString()
						};

            var totalCount = await filteredProjects.CountAsync();

            return new PagedResultDto<GetProjectForViewDto>(
                totalCount,
                await projects.ToListAsync()
            );
         }
		 
		 public async Task<GetProjectForViewDto> GetProjectForView(int id)
         {
            var project = await _projectRepository.GetAsync(id);

            var output = new GetProjectForViewDto { Project = ObjectMapper.Map<ProjectDto>(project) };

		    if (output.Project.ManagerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Project.ManagerId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Project.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Project.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.Project.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Project.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProjectForEditOutput {Project = ObjectMapper.Map<CreateOrEditProjectDto>(project)};

		    if (output.Project.ManagerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Project.ManagerId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Project.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Project.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.Project.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Project.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProjectDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Create)]
		 protected virtual async Task Create(CreateOrEditProjectDto input)
         {
            var project = ObjectMapper.Map<Project>(input);

			
			if (AbpSession.TenantId != null)
			{
				project.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _projectRepository.InsertAsync(project);
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, project);
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _projectRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input)
         {
			
			var filteredProjects = _projectRepository.GetAll()
						.Include( e => e.ManagerFk)
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.LocationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ManagerFk != null && e.ManagerFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter);

			var query = (from o in filteredProjects
                         join o1 in _lookup_userRepository.GetAll() on o.ManagerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_locationRepository.GetAll() on o.LocationId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() { 
							Project = new ProjectDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	OrganizationUnitDisplayName = s2 == null ? "" : s2.DisplayName.ToString(),
                         	LocationTitleEn = s3 == null ? "" : s3.TitleEn.ToString()
						 });


            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ProjectUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ProjectUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new ProjectOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<ProjectOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_locationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.TitleEn.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var locationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProjectLocationLookupTableDto>();
			foreach(var location in locationList){
				lookupTableDtoList.Add(new ProjectLocationLookupTableDto
				{
					Id = location.Id,
					DisplayName = location.TitleEn?.ToString()
				});
			}

            return new PagedResultDto<ProjectLocationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}