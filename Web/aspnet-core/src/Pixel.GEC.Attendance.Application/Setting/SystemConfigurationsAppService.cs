

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
	[AbpAuthorize(AppPermissions.Pages_SystemConfigurations)]
    public class SystemConfigurationsAppService : AttendanceAppServiceBase, ISystemConfigurationsAppService
    {
		 private readonly IRepository<SystemConfiguration> _systemConfigurationRepository;
		 private readonly ISystemConfigurationsExcelExporter _systemConfigurationsExcelExporter;
		 

		  public SystemConfigurationsAppService(IRepository<SystemConfiguration> systemConfigurationRepository, ISystemConfigurationsExcelExporter systemConfigurationsExcelExporter ) 
		  {
			_systemConfigurationRepository = systemConfigurationRepository;
			_systemConfigurationsExcelExporter = systemConfigurationsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetSystemConfigurationForViewDto>> GetAll(GetAllSystemConfigurationsInput input)
         {
			
			var filteredSystemConfigurations = _systemConfigurationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false );

			var pagedAndFilteredSystemConfigurations = filteredSystemConfigurations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var systemConfigurations = from o in pagedAndFilteredSystemConfigurations
                         select new GetSystemConfigurationForViewDto() {
							SystemConfiguration = new SystemConfigurationDto
							{
                                TotalPermissionNumberPerMonth = o.TotalPermissionNumberPerMonth,
                                TotalPermissionNumberPerWeek = o.TotalPermissionNumberPerWeek,
                                TotalPermissionNumberPerDay = o.TotalPermissionNumberPerDay,
                                TotalPermissionHoursPerMonth = o.TotalPermissionHoursPerMonth,
                                TotalPermissionHoursPerWeek = o.TotalPermissionHoursPerWeek,
                                TotalPermissionHoursPerDay = o.TotalPermissionHoursPerDay,
                                Id = o.Id
							}
						};

            var totalCount = await filteredSystemConfigurations.CountAsync();

            return new PagedResultDto<GetSystemConfigurationForViewDto>(
                totalCount,
                await systemConfigurations.ToListAsync()
            );
         }
		 
		 public async Task<GetSystemConfigurationForViewDto> GetSystemConfigurationForView(int id)
         {
            var systemConfiguration = await _systemConfigurationRepository.GetAsync(id);

            var output = new GetSystemConfigurationForViewDto { SystemConfiguration = ObjectMapper.Map<SystemConfigurationDto>(systemConfiguration) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SystemConfigurations_Edit)]
		 public async Task<GetSystemConfigurationForEditOutput> GetSystemConfigurationForEdit(EntityDto input)
         {
            var systemConfiguration = await _systemConfigurationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSystemConfigurationForEditOutput {SystemConfiguration = ObjectMapper.Map<CreateOrEditSystemConfigurationDto>(systemConfiguration)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSystemConfigurationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemConfigurations_Create)]
		 protected virtual async Task Create(CreateOrEditSystemConfigurationDto input)
         {
            var systemConfiguration = ObjectMapper.Map<SystemConfiguration>(input);

			

            await _systemConfigurationRepository.InsertAsync(systemConfiguration);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemConfigurations_Edit)]
		 protected virtual async Task Update(CreateOrEditSystemConfigurationDto input)
         {
            var systemConfiguration = await _systemConfigurationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, systemConfiguration);
         }

		 [AbpAuthorize(AppPermissions.Pages_SystemConfigurations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _systemConfigurationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSystemConfigurationsToExcel(GetAllSystemConfigurationsForExcelInput input)
         {
			
			var filteredSystemConfigurations = _systemConfigurationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false );

			var query = (from o in filteredSystemConfigurations
                         select new GetSystemConfigurationForViewDto() { 
							SystemConfiguration = new SystemConfigurationDto
							{
                                TotalPermissionNumberPerMonth = o.TotalPermissionNumberPerMonth,
                                TotalPermissionNumberPerWeek = o.TotalPermissionNumberPerWeek,
                                TotalPermissionNumberPerDay = o.TotalPermissionNumberPerDay,
                                TotalPermissionHoursPerMonth = o.TotalPermissionHoursPerMonth,
                                TotalPermissionHoursPerWeek = o.TotalPermissionHoursPerWeek,
                                TotalPermissionHoursPerDay = o.TotalPermissionHoursPerDay,
                                Id = o.Id
							}
						 });


            var systemConfigurationListDtos = await query.ToListAsync();

            return _systemConfigurationsExcelExporter.ExportToFile(systemConfigurationListDtos);
         }


    }
}