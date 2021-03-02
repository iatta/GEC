

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
	[AbpAuthorize(AppPermissions.Pages_TaskTypes)]
    public class TaskTypesAppService : AttendanceAppServiceBase, ITaskTypesAppService
    {
		 private readonly IRepository<TaskType> _taskTypeRepository;
		 private readonly ITaskTypesExcelExporter _taskTypesExcelExporter;
		 

		  public TaskTypesAppService(IRepository<TaskType> taskTypeRepository, ITaskTypesExcelExporter taskTypesExcelExporter ) 
		  {
			_taskTypeRepository = taskTypeRepository;
			_taskTypesExcelExporter = taskTypesExcelExporter;
			
		  }
		public async Task<List<TaskTypeDto>> GetAllFlat()
		{

			var taskTypesList = _taskTypeRepository.GetAll();

			var taskTypes = from o in taskTypesList
							select new TaskTypeDto()
							{
								
									NameAr = o.NameAr,
									NameEn = o.NameEn,
									Number = o.Number,
									Id = o.Id
								
							};


			return taskTypes.ToList();
		}

		public async Task<PagedResultDto<GetTaskTypeForViewDto>> GetAll(GetAllTaskTypesInput input)
         {
			
			var filteredTaskTypes = _taskTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Number.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter);

			var pagedAndFilteredTaskTypes = filteredTaskTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var taskTypes = from o in pagedAndFilteredTaskTypes
                         select new GetTaskTypeForViewDto() {
							TaskType = new TaskTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Number = o.Number,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTaskTypes.CountAsync();

            return new PagedResultDto<GetTaskTypeForViewDto>(
                totalCount,
                await taskTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetTaskTypeForViewDto> GetTaskTypeForView(int id)
         {
            var taskType = await _taskTypeRepository.GetAsync(id);

            var output = new GetTaskTypeForViewDto { TaskType = ObjectMapper.Map<TaskTypeDto>(taskType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TaskTypes_Edit)]
		 public async Task<GetTaskTypeForEditOutput> GetTaskTypeForEdit(EntityDto input)
         {
            var taskType = await _taskTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTaskTypeForEditOutput {TaskType = ObjectMapper.Map<CreateOrEditTaskTypeDto>(taskType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTaskTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TaskTypes_Create)]
		 protected virtual async Task Create(CreateOrEditTaskTypeDto input)
         {
            var taskType = ObjectMapper.Map<TaskType>(input);

			

            await _taskTypeRepository.InsertAsync(taskType);
         }

		 [AbpAuthorize(AppPermissions.Pages_TaskTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditTaskTypeDto input)
         {
            var taskType = await _taskTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, taskType);
         }

		 [AbpAuthorize(AppPermissions.Pages_TaskTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _taskTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTaskTypesToExcel(GetAllTaskTypesForExcelInput input)
         {
			
			var filteredTaskTypes = _taskTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Number.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter);

			var query = (from o in filteredTaskTypes
                         select new GetTaskTypeForViewDto() { 
							TaskType = new TaskTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Number = o.Number,
                                Id = o.Id
							}
						 });


            var taskTypeListDtos = await query.ToListAsync();

            return _taskTypesExcelExporter.ExportToFile(taskTypeListDtos);
         }


    }
}