

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.Setting
{
	[AbpAuthorize(AppPermissions.Pages_Nationalities)]
    public class NationalitiesAppService : AttendanceAppServiceBase, INationalitiesAppService
    {
		 private readonly IRepository<Nationality> _nationalityRepository;
		 

		  public NationalitiesAppService(IRepository<Nationality> nationalityRepository ) 
		  {
			_nationalityRepository = nationalityRepository;
			
		  }

		 public async Task<PagedResultDto<GetNationalityForViewDto>> GetAll(GetAllNationalitiesInput input)
         {
			
			var filteredNationalities = _nationalityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter));

			var pagedAndFilteredNationalities = filteredNationalities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var nationalities = from o in pagedAndFilteredNationalities
                         select new GetNationalityForViewDto() {
							Nationality = new NationalityDto
							{
                                Id = o.Id
							}
						};

            var totalCount = await filteredNationalities.CountAsync();

            return new PagedResultDto<GetNationalityForViewDto>(
                totalCount,
                await nationalities.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Nationalities_Edit)]
		 public async Task<GetNationalityForEditOutput> GetNationalityForEdit(EntityDto input)
         {
            var nationality = await _nationalityRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetNationalityForEditOutput {Nationality = ObjectMapper.Map<CreateOrEditNationalityDto>(nationality)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditNationalityDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Nationalities_Create)]
		 protected virtual async Task Create(CreateOrEditNationalityDto input)
         {
            var nationality = ObjectMapper.Map<Nationality>(input);

			

            await _nationalityRepository.InsertAsync(nationality);
         }

		 [AbpAuthorize(AppPermissions.Pages_Nationalities_Edit)]
		 protected virtual async Task Update(CreateOrEditNationalityDto input)
         {
            var nationality = await _nationalityRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, nationality);
         }

		 [AbpAuthorize(AppPermissions.Pages_Nationalities_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _nationalityRepository.DeleteAsync(input.Id);
         } 
    }
}