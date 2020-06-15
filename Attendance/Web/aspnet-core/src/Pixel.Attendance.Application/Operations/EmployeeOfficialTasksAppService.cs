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
	[AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks)]
    public class EmployeeOfficialTasksAppService : AttendanceAppServiceBase, IEmployeeOfficialTasksAppService
    {
		 private readonly IRepository<EmployeeOfficialTask> _employeeOfficialTaskRepository;
		 private readonly IEmployeeOfficialTasksExcelExporter _employeeOfficialTasksExcelExporter;
		 private readonly IRepository<OfficialTaskType,int> _lookup_officialTaskTypeRepository;
		 

		  public EmployeeOfficialTasksAppService(IRepository<EmployeeOfficialTask> employeeOfficialTaskRepository, IEmployeeOfficialTasksExcelExporter employeeOfficialTasksExcelExporter , IRepository<OfficialTaskType, int> lookup_officialTaskTypeRepository) 
		  {
			_employeeOfficialTaskRepository = employeeOfficialTaskRepository;
			_employeeOfficialTasksExcelExporter = employeeOfficialTasksExcelExporter;
			_lookup_officialTaskTypeRepository = lookup_officialTaskTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeeOfficialTaskForViewDto>> GetAll(GetAllEmployeeOfficialTasksInput input)
         {
			
			var filteredEmployeeOfficialTasks = _employeeOfficialTaskRepository.GetAll()
						.Include( e => e.OfficialTaskTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remarks.Contains(input.Filter) || e.DescriptionAr.Contains(input.Filter) || e.DescriptionEn.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OfficialTaskTypeNameArFilter), e => e.OfficialTaskTypeFk != null && e.OfficialTaskTypeFk.NameAr == input.OfficialTaskTypeNameArFilter);

			var pagedAndFilteredEmployeeOfficialTasks = filteredEmployeeOfficialTasks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeOfficialTasks = from o in pagedAndFilteredEmployeeOfficialTasks
                         join o1 in _lookup_officialTaskTypeRepository.GetAll() on o.OfficialTaskTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetEmployeeOfficialTaskForViewDto() {
							EmployeeOfficialTask = new EmployeeOfficialTaskDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Remarks = o.Remarks,
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                Id = o.Id
							},
                         	OfficialTaskTypeNameAr = s1 == null ? "" : s1.NameAr.ToString()
						};

            var totalCount = await filteredEmployeeOfficialTasks.CountAsync();

            return new PagedResultDto<GetEmployeeOfficialTaskForViewDto>(
                totalCount,
                await employeeOfficialTasks.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeOfficialTaskForViewDto> GetEmployeeOfficialTaskForView(int id)
         {
            var employeeOfficialTask = await _employeeOfficialTaskRepository.GetAsync(id);

            var output = new GetEmployeeOfficialTaskForViewDto { EmployeeOfficialTask = ObjectMapper.Map<EmployeeOfficialTaskDto>(employeeOfficialTask) };

		    if (output.EmployeeOfficialTask.OfficialTaskTypeId != null)
            {
                var _lookupOfficialTaskType = await _lookup_officialTaskTypeRepository.FirstOrDefaultAsync((int)output.EmployeeOfficialTask.OfficialTaskTypeId);
                output.OfficialTaskTypeNameAr = _lookupOfficialTaskType.NameAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks_Edit)]
		 public async Task<GetEmployeeOfficialTaskForEditOutput> GetEmployeeOfficialTaskForEdit(EntityDto input)
         {
            var employeeOfficialTask = await _employeeOfficialTaskRepository.GetAllIncluding(x => x.OfficialTaskDetails).FirstOrDefaultAsync(x => x.Id == input.Id);
           
		    var output = new GetEmployeeOfficialTaskForEditOutput {EmployeeOfficialTask = ObjectMapper.Map<CreateOrEditEmployeeOfficialTaskDto>(employeeOfficialTask)};

		    if (output.EmployeeOfficialTask.OfficialTaskTypeId != null)
            {
                var _lookupOfficialTaskType = await _lookup_officialTaskTypeRepository.FirstOrDefaultAsync((int)output.EmployeeOfficialTask.OfficialTaskTypeId);
                output.OfficialTaskTypeNameAr = _lookupOfficialTaskType.NameAr.ToString();
            }
            output.EmployeeOfficialTask.SelectedUsers = new List<long?>(employeeOfficialTask.OfficialTaskDetails.Select(x => x.UserId));
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeOfficialTaskDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeOfficialTaskDto input)
         {
            if (input.SelectedUsers != null)
            {
                input.OfficialTaskDetails = new List<EmployeeOfficialTaskDetailDto>();
                foreach (var item in input.SelectedUsers)
                {
                    input.OfficialTaskDetails.Add(new EmployeeOfficialTaskDetailDto()
                    {
                        EmployeeOfficialTaskId = input.Id,
                        UserId = item
                    });
                }
            }


            var employeeOfficialTask = ObjectMapper.Map<EmployeeOfficialTask>(input);

            employeeOfficialTask.OfficialTaskDetails = ObjectMapper.Map<List<EmployeeOfficialTaskDetail>>(input.OfficialTaskDetails);

            await _employeeOfficialTaskRepository.InsertAsync(employeeOfficialTask);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeOfficialTaskDto input)
         {
          
            var employeeOfficialTask = await _employeeOfficialTaskRepository.GetAllIncluding(x => x.OfficialTaskDetails)
                                                                            .FirstOrDefaultAsync(x => x.Id == (int)input.Id);

            employeeOfficialTask.OfficialTaskDetails = null;
            if (input.SelectedUsers != null)
            {
                input.OfficialTaskDetails = new List<EmployeeOfficialTaskDetailDto>();
                foreach (var item in input.SelectedUsers)
                {
                    input.OfficialTaskDetails.Add(new EmployeeOfficialTaskDetailDto()
                    {
                        EmployeeOfficialTaskId = input.Id,
                        UserId = item

                    });
                }

            }


            ObjectMapper.Map(input, employeeOfficialTask);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeOfficialTaskRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeOfficialTasksToExcel(GetAllEmployeeOfficialTasksForExcelInput input)
         {
			
			var filteredEmployeeOfficialTasks = _employeeOfficialTaskRepository.GetAll()
						.Include( e => e.OfficialTaskTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Remarks.Contains(input.Filter) || e.DescriptionAr.Contains(input.Filter) || e.DescriptionEn.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter),  e => e.Remarks == input.RemarksFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OfficialTaskTypeNameArFilter), e => e.OfficialTaskTypeFk != null && e.OfficialTaskTypeFk.NameAr == input.OfficialTaskTypeNameArFilter);

			var query = (from o in filteredEmployeeOfficialTasks
                         join o1 in _lookup_officialTaskTypeRepository.GetAll() on o.OfficialTaskTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetEmployeeOfficialTaskForViewDto() { 
							EmployeeOfficialTask = new EmployeeOfficialTaskDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Remarks = o.Remarks,
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                Id = o.Id
							},
                         	OfficialTaskTypeNameAr = s1 == null ? "" : s1.NameAr.ToString()
						 });


            var employeeOfficialTaskListDtos = await query.ToListAsync();

            return _employeeOfficialTasksExcelExporter.ExportToFile(employeeOfficialTaskListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeOfficialTasks)]
         public async Task<PagedResultDto<EmployeeOfficialTaskOfficialTaskTypeLookupTableDto>> GetAllOfficialTaskTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_officialTaskTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var officialTaskTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeOfficialTaskOfficialTaskTypeLookupTableDto>();
			foreach(var officialTaskType in officialTaskTypeList){
				lookupTableDtoList.Add(new EmployeeOfficialTaskOfficialTaskTypeLookupTableDto
				{
					Id = officialTaskType.Id,
					DisplayName = officialTaskType.NameAr?.ToString()
				});
			}

            return new PagedResultDto<EmployeeOfficialTaskOfficialTaskTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}