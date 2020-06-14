

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
	[AbpAuthorize(AppPermissions.Pages_JobTitles)]
    public class JobTitleAppService : AttendanceAppServiceBase, IJobTitlesAppService
    {
		 private readonly IRepository<JobTitle> _jobTitleRepository;
		 private readonly IJobTitleExcelExporter _jobTitleExcelExporter;
		 

		  public JobTitleAppService(IRepository<JobTitle> jobTitleRepository, IJobTitleExcelExporter testsExcelExporter ) 
		  {
			_jobTitleRepository = jobTitleRepository;
			_jobTitleExcelExporter = testsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetJobTitleForViewDto>> GetAll(GetAllJobTitlesInput input)
         {
			
			var filteredTests = _jobTitleRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter);

			var pagedAndFilteredTests = filteredTests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var tests = from o in pagedAndFilteredTests
                         select new GetJobTitleForViewDto() {
							JobTitle = new JobTitleDto
							{
                                NameAr = o.NameAr,
								NameEn = o.NameEn,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTests.CountAsync();

            return new PagedResultDto<GetJobTitleForViewDto>(
                totalCount,
                await tests.ToListAsync()
            );
         }

		public async Task<List<JobTitleDto>> GetAllFlat()
		{

			var filteredTests = _jobTitleRepository.GetAll();

			var tests = from o in filteredTests
						select new JobTitleDto()
						{
							
								NameAr = o.NameEn,
								NameEn = o.NameEn,
								Id = o.Id
							
						};

			

			var output =  new List<JobTitleDto>(await tests.ToListAsync());
			return output;
		}

		public async Task<GetJobTitleForViewDto> GetJobTitleForView(int id)
         {
            var jobTitle = await _jobTitleRepository.GetAsync(id);

            var output = new GetJobTitleForViewDto { JobTitle = ObjectMapper.Map<JobTitleDto>(jobTitle) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JobTitles_Edit)]
		 public async Task<GetJobTitleForEditOutput> GetJobTitleForEdit(EntityDto input)
         {
            var test = await _jobTitleRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJobTitleForEditOutput { JobTitle = ObjectMapper.Map<CreateOrEditJobTitleDto>(test)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJobTitleDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JobTitles_Create)]
		 protected virtual async Task Create(CreateOrEditJobTitleDto input)
         {
            var test = ObjectMapper.Map<JobTitle>(input);

			

            await _jobTitleRepository.InsertAsync(test);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobTitles_Edit)]
		 protected virtual async Task Update(CreateOrEditJobTitleDto input)
         {
            var jobTitle = await _jobTitleRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jobTitle);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobTitles_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jobTitleRepository.DeleteAsync(input.Id);
         }
        public  string GetTitleById(int id , int lang)
        {
            var res = _jobTitleRepository.Get(id);
            if (res == null)
            {
                return "";
            }
            return (lang == 1) ? res.NameAr : res.NameEn;
        }

        public async Task<FileDto> GetJobTitlesToExcel(GetAllJobTitlesForExcelInput input)
         {
			
			var filteredTests = _jobTitleRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter);

			var query = (from o in filteredTests
                         select new GetJobTitleForViewDto() { 
							JobTitle = new JobTitleDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							}
						 });


            var jobTitleListDtos = await query.ToListAsync();

            return _jobTitleExcelExporter.ExportToFile(jobTitleListDtos);
         }


    }
}