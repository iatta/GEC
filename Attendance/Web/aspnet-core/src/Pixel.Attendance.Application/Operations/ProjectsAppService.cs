using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;
using Abp.Organizations;


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
using Pixel.Attendance.Extended;

namespace Pixel.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_Projects)]
    public class ProjectsAppService : AttendanceAppServiceBase, IProjectsAppService
    {
        
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectLocation> _projectLocationRepository;
        private readonly IProjectsExcelExporter _projectsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Location,int> _lookup_locationRepository;
		 private readonly IRepository<OrganizationUnitExtended,long> _lookup_organizationUnitRepository;
        


          public ProjectsAppService(IRepository<ProjectLocation> projectLocationRepository , IRepository<Project> projectRepository, IProjectsExcelExporter projectsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Location, int> lookup_locationRepository, IRepository<OrganizationUnitExtended, long> lookup_organizationUnitRepository) 
		  {
			_projectRepository = projectRepository;
			_projectsExcelExporter = projectsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_locationRepository = lookup_locationRepository;
		_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _projectLocationRepository = projectLocationRepository;


          }

        public async Task<List<ProjectDto>> GetAllFlatForHr()
        {

            var data = await _projectRepository.GetAllListAsync();
            return ObjectMapper.Map<List<ProjectDto>>(data);
        }

        public async Task<List<ProjectDto>> GetAllFlatForProjectManager()
        {

            var data = await _projectRepository.GetAll().Where(x => x.ManagerId == GetCurrentUser().Id).ToListAsync();

            return ObjectMapper.Map<List<ProjectDto>>(data);
        }

        public async Task<List<ProjectDto>> GetAllFlatForOrganizationUnitManager()
        {
            var units = new List<long>();
          
            var curentUserUnits = await _lookup_organizationUnitRepository.GetAllIncluding(x => x.Children).Where(u => u.ManagerId == GetCurrentUser().Id).ToListAsync();
            var userunitIds = curentUserUnits.Select(x => x.Id).ToList();
            var allUnits = _lookup_organizationUnitRepository.GetAll().ToList();
            units.AddRange(userunitIds);
            foreach (var unit in curentUserUnits)
            {
                var childUnits = new List<long>();
                childUnits = GetChildes(childUnits, unit, allUnits);
                units.AddRange(childUnits);
            }
           
           
            
            var data = await _projectRepository.GetAll().Where(x => units.Contains(x.OrganizationUnitId.Value)).ToListAsync();
            return ObjectMapper.Map<List<ProjectDto>>(data);

           
        }
        public async Task<List<ProjectDto>> GetAllFlat()
        {
            var data = await _projectRepository.GetAll().ToListAsync();
            return ObjectMapper.Map<List<ProjectDto>>(data);
        }


        public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
         {
			
			var filteredProjects = _projectRepository.GetAll()
						.Include( e => e.ManagerFk)
						.Include( e => e.LocationFk)
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ManagerFk != null && e.ManagerFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var projects = from o in pagedAndFilteredProjects
                         join o1 in _lookup_userRepository.GetAll() on o.ManagerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() {
							Project = new ProjectDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id,
                                Code = o.Code,
                                Number = o.Number
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString(),
                         	OrganizationUnitDisplayName = s3 == null ? "" : s3.DisplayName.ToString()
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

		    if (output.Project.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Project.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }

		    if (output.Project.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Project.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);
            project.Locations = _projectLocationRepository.GetAll().Where(x => x.ProjectId == project.Id).ToList();

            var output = new GetProjectForEditOutput {Project = ObjectMapper.Map<CreateOrEditProjectDto>(project)};

		    if (output.Project.ManagerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Project.ManagerId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Project.LocationId != null)
            {
                var _lookupLocation = await _lookup_locationRepository.FirstOrDefaultAsync((int)output.Project.LocationId);
                output.LocationTitleEn = _lookupLocation.TitleEn.ToString();
            }

		    if (output.Project.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Project.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
            foreach (var projectLocation in output.Project.Locations)
            {
                var location = _lookup_locationRepository.FirstOrDefault(x => x.Id == projectLocation.Id);
                if(location != null)
                    projectLocation.LocationName =location.TitleEn;

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
            project.Locations = new List<ProjectLocation>();

            if (input.Locations.Count > 0)
            {
                foreach (var item in input.Locations)
                {
                    project.Locations.Add(ObjectMapper.Map<ProjectLocation>(item));
                }

            }


            await _projectRepository.InsertAsync(project);
         }

		 [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
		 protected virtual async Task Update(CreateOrEditProjectDto input)
         {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);
            project.Locations = _projectLocationRepository.GetAll().Where(x => x.ProjectId == project.Id).ToList();

            var oldProjectLocations = new HashSet<ProjectLocation>(project.Locations.ToList());
            var newProjectLocations = new HashSet<ProjectLocationDto>(input.Locations.ToList());

            foreach (var detail in oldProjectLocations)
            {
                if (!newProjectLocations.Any(x => x.Id == detail.Id))
                {
                    project.Locations.Remove(detail);
                }
                else
                {
                    var inputDetail = newProjectLocations.Where(x => x.Id == detail.Id).FirstOrDefault();
                }

            }

            foreach (var item in newProjectLocations)
            {
                if (item.Id == 0)
                {
                    project.Locations.Add(ObjectMapper.Map<ProjectLocation>(item));
                }
            }
            await _projectRepository.UpdateAsync(project);
            
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
						.Include( e => e.LocationFk)
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ManagerFk != null && e.ManagerFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleEnFilter), e => e.LocationFk != null && e.LocationFk.TitleEn == input.LocationTitleEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var query = (from o in filteredProjects
                         join o1 in _lookup_userRepository.GetAll() on o.ManagerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_locationRepository.GetAll() on o.LocationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProjectForViewDto() { 
							Project = new ProjectDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id,
                                Code = o.Code,
                                Number = o.Number
                                
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	LocationTitleEn = s2 == null ? "" : s2.TitleEn.ToString(),
                         	OrganizationUnitDisplayName = s3 == null ? "" : s3.DisplayName.ToString()
						 });


            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
            var query = _lookup_userRepository.GetAll();
            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(x => x.Name.ToLower().Contains(input.Filter.ToLower()));
            }

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
         public async Task<PagedResultDto<ProjectLocationLookupTableDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
         {
            var query = _lookup_locationRepository.GetAll();

            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(x => x.TitleEn.ToLower().Contains(input.Filter.ToLower()));
            }

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

       

        [AbpAuthorize(AppPermissions.Pages_Projects)]
         public async Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
            var query = _lookup_organizationUnitRepository.GetAll();
            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(x => x.DisplayName.ToLower().Contains(input.Filter.ToLower()));
            }

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




        //project users 
        public async Task<List<ProjectUserDto>> GetProjectUsers(int projectId)
        {
            var output = new List<ProjectUserDto>();
            var project = await _projectRepository.GetAllIncluding(x => x.Users).FirstOrDefaultAsync(x => x.Id == projectId);

            foreach (var projectUser in project.Users)
            {
                output.Add(new ProjectUserDto { ProjectId = project.Id, UserId = projectUser.UserId });
            }
            return output;
        }


        public async Task UpdateProjectUsers(ProjectUserInputDto input)
        {
            var project = await _projectRepository.GetAllIncluding(x => x.Users).FirstOrDefaultAsync(x => x.Id == input.ProjectId);
            project.Users.Clear();

            foreach (var projectUser in input.ProjectUsers)
            {
                project.Users.Add(new ProjectUser { ProjectId = project.Id, UserId = projectUser.UserId });
            }

            await _projectRepository.UpdateAsync(project);

        }


        //project machines 
        public async Task<List<ProjectMachineDto>> GetProjecMachines(int projectId)
        {
            var output = new List<ProjectMachineDto>();
            var project = await _projectRepository.GetAllIncluding(x => x.Machines).FirstOrDefaultAsync(x => x.Id == projectId);

            foreach (var projecMachine in project.Machines)
            {
                output.Add(new ProjectMachineDto { ProjectId = project.Id, MachineId = projecMachine.MachineId });
            }
            return output;
        }


        public async Task UpdateProjecMachines(ProjectMachineInputDto input)
        {
            var project = await _projectRepository.GetAllIncluding(x => x.Machines).FirstOrDefaultAsync(x => x.Id == input.ProjectId);
            project.Machines.Clear();

            foreach (var projectUser in input.ProjectMachines)
            {
                project.Machines.Add(new ProjectMachine { ProjectId = project.Id, MachineId = projectUser.MachineId });
            }

            await _projectRepository.UpdateAsync(project);

        }

        private  List<long> GetChildes(List<long> childs, OrganizationUnitExtended unit, List<OrganizationUnitExtended> units)
        {
            if (unit.Children.Count > 0)
            {
                foreach (var child in unit.Children)
                {
                    childs.Add(child.Id);
                    var newEntity = _lookup_organizationUnitRepository.GetAllIncluding(x => x.Children).FirstOrDefault(d => d.Id == child.Id);
                    if (newEntity.Children.Count > 0)
                    {
                        GetChildes(childs, newEntity, units);
                    }
                }
                
            }

            return childs;



        }
        //public static List<long> GetChildren(List<long> childs, int parentId)
        //{
        //    return comments
        //            .Where(c => c.ParentId == parentId)
        //            .Select(c => new Comment
        //            {
        //                Id = c.Id,
        //                Text = c.Text,
        //                ParentId = c.ParentId,
        //                Children = GetChildren(comments, c.Id)
        //            })
        //            .ToList();
        //}

    }
}