

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
	[AbpAuthorize(AppPermissions.Pages_WarningTypes)]
    public class WarningTypesAppService : AttendanceAppServiceBase, IWarningTypesAppService
    {
		 private readonly IRepository<WarningType> _warningTypeRepository;
		 private readonly IWarningTypesExcelExporter _warningTypesExcelExporter;
		 

		  public WarningTypesAppService(IRepository<WarningType> warningTypeRepository, IWarningTypesExcelExporter warningTypesExcelExporter ) 
		  {
			_warningTypeRepository = warningTypeRepository;
			_warningTypesExcelExporter = warningTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetWarningTypeForViewDto>> GetAll(GetAllWarningTypesInput input)
         {
			
			var filteredWarningTypes = _warningTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter);

			var pagedAndFilteredWarningTypes = filteredWarningTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var warningTypes = from o in pagedAndFilteredWarningTypes
                         select new GetWarningTypeForViewDto() {
							WarningType = new WarningTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							}
						};

            var totalCount = await filteredWarningTypes.CountAsync();

            return new PagedResultDto<GetWarningTypeForViewDto>(
                totalCount,
                await warningTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetWarningTypeForViewDto> GetWarningTypeForView(int id)
         {
            var warningType = await _warningTypeRepository.GetAsync(id);

            var output = new GetWarningTypeForViewDto { WarningType = ObjectMapper.Map<WarningTypeDto>(warningType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_WarningTypes_Edit)]
		 public async Task<GetWarningTypeForEditOutput> GetWarningTypeForEdit(EntityDto input)
         {
            var warningType = await _warningTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetWarningTypeForEditOutput {WarningType = ObjectMapper.Map<CreateOrEditWarningTypeDto>(warningType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditWarningTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_WarningTypes_Create)]
		 protected virtual async Task Create(CreateOrEditWarningTypeDto input)
         {
            var warningType = ObjectMapper.Map<WarningType>(input);

			

            await _warningTypeRepository.InsertAsync(warningType);
         }

		 [AbpAuthorize(AppPermissions.Pages_WarningTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditWarningTypeDto input)
         {
            var warningType = await _warningTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, warningType);
         }

		 [AbpAuthorize(AppPermissions.Pages_WarningTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _warningTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetWarningTypesToExcel(GetAllWarningTypesForExcelInput input)
         {
			
			var filteredWarningTypes = _warningTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter);

			var query = (from o in filteredWarningTypes
                         select new GetWarningTypeForViewDto() { 
							WarningType = new WarningTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							}
						 });


            var warningTypeListDtos = await query.ToListAsync();

            return _warningTypesExcelExporter.ExportToFile(warningTypeListDtos);
         }


    }
}