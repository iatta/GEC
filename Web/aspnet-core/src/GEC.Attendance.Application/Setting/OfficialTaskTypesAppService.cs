

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
	[AbpAuthorize(AppPermissions.Pages_OfficialTaskTypes)]
    public class OfficialTaskTypesAppService : AttendanceAppServiceBase, IOfficialTaskTypesAppService
    {
		 private readonly IRepository<OfficialTaskType> _officialTaskTypeRepository;
		 private readonly IOfficialTaskTypesExcelExporter _officialTaskTypesExcelExporter;
		 

		  public OfficialTaskTypesAppService(IRepository<OfficialTaskType> officialTaskTypeRepository, IOfficialTaskTypesExcelExporter officialTaskTypesExcelExporter ) 
		  {
			_officialTaskTypeRepository = officialTaskTypeRepository;
			_officialTaskTypesExcelExporter = officialTaskTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetOfficialTaskTypeForViewDto>> GetAll(GetAllOfficialTaskTypesInput input)
         {
			
			var filteredOfficialTaskTypes = _officialTaskTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(input.TypeInFilter > -1,  e => (input.TypeInFilter == 1 && e.TypeIn) || (input.TypeInFilter == 0 && !e.TypeIn) )
						.WhereIf(input.TypeOutFilter > -1,  e => (input.TypeOutFilter == 1 && e.TypeOut) || (input.TypeOutFilter == 0 && !e.TypeOut) )
						.WhereIf(input.TypeInOutFilter > -1,  e => (input.TypeInOutFilter == 1 && e.TypeInOut) || (input.TypeInOutFilter == 0 && !e.TypeInOut) );

			var pagedAndFilteredOfficialTaskTypes = filteredOfficialTaskTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var officialTaskTypes = from o in pagedAndFilteredOfficialTaskTypes
                         select new GetOfficialTaskTypeForViewDto() {
							OfficialTaskType = new OfficialTaskTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                TypeIn = o.TypeIn,
                                TypeOut = o.TypeOut,
                                TypeInOut = o.TypeInOut,
                                Id = o.Id
							}
						};

            var totalCount = await filteredOfficialTaskTypes.CountAsync();

            return new PagedResultDto<GetOfficialTaskTypeForViewDto>(
                totalCount,
                await officialTaskTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetOfficialTaskTypeForViewDto> GetOfficialTaskTypeForView(int id)
         {
            var officialTaskType = await _officialTaskTypeRepository.GetAsync(id);

            var output = new GetOfficialTaskTypeForViewDto { OfficialTaskType = ObjectMapper.Map<OfficialTaskTypeDto>(officialTaskType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_OfficialTaskTypes_Edit)]
		 public async Task<GetOfficialTaskTypeForEditOutput> GetOfficialTaskTypeForEdit(EntityDto input)
         {
            var officialTaskType = await _officialTaskTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetOfficialTaskTypeForEditOutput {OfficialTaskType = ObjectMapper.Map<CreateOrEditOfficialTaskTypeDto>(officialTaskType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditOfficialTaskTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_OfficialTaskTypes_Create)]
		 protected virtual async Task Create(CreateOrEditOfficialTaskTypeDto input)
         {
            var officialTaskType = ObjectMapper.Map<OfficialTaskType>(input);

			

            await _officialTaskTypeRepository.InsertAsync(officialTaskType);
         }

		 [AbpAuthorize(AppPermissions.Pages_OfficialTaskTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditOfficialTaskTypeDto input)
         {
            var officialTaskType = await _officialTaskTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, officialTaskType);
         }

		 [AbpAuthorize(AppPermissions.Pages_OfficialTaskTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _officialTaskTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetOfficialTaskTypesToExcel(GetAllOfficialTaskTypesForExcelInput input)
         {
			
			var filteredOfficialTaskTypes = _officialTaskTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(input.TypeInFilter > -1,  e => (input.TypeInFilter == 1 && e.TypeIn) || (input.TypeInFilter == 0 && !e.TypeIn) )
						.WhereIf(input.TypeOutFilter > -1,  e => (input.TypeOutFilter == 1 && e.TypeOut) || (input.TypeOutFilter == 0 && !e.TypeOut) )
						.WhereIf(input.TypeInOutFilter > -1,  e => (input.TypeInOutFilter == 1 && e.TypeInOut) || (input.TypeInOutFilter == 0 && !e.TypeInOut) );

			var query = (from o in filteredOfficialTaskTypes
                         select new GetOfficialTaskTypeForViewDto() { 
							OfficialTaskType = new OfficialTaskTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                TypeIn = o.TypeIn,
                                TypeOut = o.TypeOut,
                                TypeInOut = o.TypeInOut,
                                Id = o.Id
							}
						 });


            var officialTaskTypeListDtos = await query.ToListAsync();

            return _officialTaskTypesExcelExporter.ExportToFile(officialTaskTypeListDtos);
         }


    }
}