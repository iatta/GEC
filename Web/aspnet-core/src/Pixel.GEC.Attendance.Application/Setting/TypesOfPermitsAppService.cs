

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
	[AbpAuthorize(AppPermissions.Pages_TypesOfPermits)]
    public class TypesOfPermitsAppService : AttendanceAppServiceBase, ITypesOfPermitsAppService
    {
		 private readonly IRepository<TypesOfPermit> _typesOfPermitRepository;
		 private readonly ITypesOfPermitsExcelExporter _typesOfPermitsExcelExporter;
		 

		  public TypesOfPermitsAppService(IRepository<TypesOfPermit> typesOfPermitRepository, ITypesOfPermitsExcelExporter typesOfPermitsExcelExporter ) 
		  {
			_typesOfPermitRepository = typesOfPermitRepository;
			_typesOfPermitsExcelExporter = typesOfPermitsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetTypesOfPermitForViewDto>> GetAll(GetAllTypesOfPermitsInput input)
         {
			
			var filteredTypesOfPermits = _typesOfPermitRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter));

			var pagedAndFilteredTypesOfPermits = filteredTypesOfPermits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var typesOfPermits = from o in pagedAndFilteredTypesOfPermits
                         select new GetTypesOfPermitForViewDto() {
							TypesOfPermit = new TypesOfPermitDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTypesOfPermits.CountAsync();

            return new PagedResultDto<GetTypesOfPermitForViewDto>(
                totalCount,
                await typesOfPermits.ToListAsync()
            );
         }

        public async Task<List<GetTypesOfPermitForViewDto>> GetAllFlat()
        {

            var filteredTypesOfPermits = _typesOfPermitRepository.GetAll();
            var typesOfPermits = from o in filteredTypesOfPermits
                                 select new GetTypesOfPermitForViewDto()
                                 {
                                     TypesOfPermit = new TypesOfPermitDto
                                     {
                                         NameAr = o.NameAr,
                                         NameEn = o.NameEn,
                                         Id = o.Id
                                     }
                                 };

            return new List<GetTypesOfPermitForViewDto>(await typesOfPermits.ToListAsync());
        }



        public async Task<GetTypesOfPermitForViewDto> GetTypesOfPermitForView(int id)
         {
            var typesOfPermit = await _typesOfPermitRepository.GetAsync(id);

            var output = new GetTypesOfPermitForViewDto { TypesOfPermit = ObjectMapper.Map<TypesOfPermitDto>(typesOfPermit) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TypesOfPermits_Edit)]
		 public async Task<GetTypesOfPermitForEditOutput> GetTypesOfPermitForEdit(EntityDto input)
         {
            var typesOfPermit = await _typesOfPermitRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTypesOfPermitForEditOutput {TypesOfPermit = ObjectMapper.Map<CreateOrEditTypesOfPermitDto>(typesOfPermit)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTypesOfPermitDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TypesOfPermits_Create)]
		 protected virtual async Task Create(CreateOrEditTypesOfPermitDto input)
         {
            var typesOfPermit = ObjectMapper.Map<TypesOfPermit>(input);

			

            await _typesOfPermitRepository.InsertAsync(typesOfPermit);
         }

		 [AbpAuthorize(AppPermissions.Pages_TypesOfPermits_Edit)]
		 protected virtual async Task Update(CreateOrEditTypesOfPermitDto input)
         {
            var typesOfPermit = await _typesOfPermitRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, typesOfPermit);
         }

		 [AbpAuthorize(AppPermissions.Pages_TypesOfPermits_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _typesOfPermitRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTypesOfPermitsToExcel(GetAllTypesOfPermitsForExcelInput input)
         {
			
			var filteredTypesOfPermits = _typesOfPermitRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter));

			var query = (from o in filteredTypesOfPermits
                         select new GetTypesOfPermitForViewDto() { 
							TypesOfPermit = new TypesOfPermitDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Id = o.Id
							}
						 });


            var typesOfPermitListDtos = await query.ToListAsync();

            return _typesOfPermitsExcelExporter.ExportToFile(typesOfPermitListDtos);
         }


    }
}