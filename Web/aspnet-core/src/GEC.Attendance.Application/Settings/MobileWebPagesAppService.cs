

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using GEC.Attendance.Settings.Exporting;
using GEC.Attendance.Settings.Dtos;
using GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GEC.Attendance.Settings
{
	[AbpAuthorize(AppPermissions.Pages_MobileWebPages)]
    public class MobileWebPagesAppService : AttendanceAppServiceBase, IMobileWebPagesAppService
    {
		 private readonly IRepository<MobileWebPage> _mobileWebPageRepository;
		 private readonly IMobileWebPagesExcelExporter _mobileWebPagesExcelExporter;
		 

		  public MobileWebPagesAppService(IRepository<MobileWebPage> mobileWebPageRepository, IMobileWebPagesExcelExporter mobileWebPagesExcelExporter ) 
		  {
			_mobileWebPageRepository = mobileWebPageRepository;
			_mobileWebPagesExcelExporter = mobileWebPagesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetMobileWebPageForViewDto>> GetAll(GetAllMobileWebPagesInput input)
         {
			
			var filteredMobileWebPages = _mobileWebPageRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Content.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter),  e => e.Content == input.ContentFilter);

			var pagedAndFilteredMobileWebPages = filteredMobileWebPages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var mobileWebPages = from o in pagedAndFilteredMobileWebPages
                         select new GetMobileWebPageForViewDto() {
							MobileWebPage = new MobileWebPageDto
							{
                                Name = o.Name,
                                Content = o.Content,
                                Id = o.Id
							}
						};

            var totalCount = await filteredMobileWebPages.CountAsync();

            return new PagedResultDto<GetMobileWebPageForViewDto>(
                totalCount,
                await mobileWebPages.ToListAsync()
            );
         }
		 
		 public async Task<GetMobileWebPageForViewDto> GetMobileWebPageForView(int id)
         {
            var mobileWebPage = await _mobileWebPageRepository.GetAsync(id);

            var output = new GetMobileWebPageForViewDto { MobileWebPage = ObjectMapper.Map<MobileWebPageDto>(mobileWebPage) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_MobileWebPages_Edit)]
		 public async Task<GetMobileWebPageForEditOutput> GetMobileWebPageForEdit(EntityDto input)
         {
            var mobileWebPage = await _mobileWebPageRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetMobileWebPageForEditOutput {MobileWebPage = ObjectMapper.Map<CreateOrEditMobileWebPageDto>(mobileWebPage)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditMobileWebPageDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_MobileWebPages_Create)]
		 protected virtual async Task Create(CreateOrEditMobileWebPageDto input)
         {
            var mobileWebPage = ObjectMapper.Map<MobileWebPage>(input);

			

            await _mobileWebPageRepository.InsertAsync(mobileWebPage);
         }

		 [AbpAuthorize(AppPermissions.Pages_MobileWebPages_Edit)]
		 protected virtual async Task Update(CreateOrEditMobileWebPageDto input)
         {
            var mobileWebPage = await _mobileWebPageRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, mobileWebPage);
         }

		 [AbpAuthorize(AppPermissions.Pages_MobileWebPages_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _mobileWebPageRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetMobileWebPagesToExcel(GetAllMobileWebPagesForExcelInput input)
         {
			
			var filteredMobileWebPages = _mobileWebPageRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Content.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter),  e => e.Content == input.ContentFilter);

			var query = (from o in filteredMobileWebPages
                         select new GetMobileWebPageForViewDto() { 
							MobileWebPage = new MobileWebPageDto
							{
                                Name = o.Name,
                                Content = o.Content,
                                Id = o.Id
							}
						 });


            var mobileWebPageListDtos = await query.ToListAsync();

            return _mobileWebPagesExcelExporter.ExportToFile(mobileWebPageListDtos);
         }

		public async Task<GetMobileWebPageForViewDto> GetMobileWebPageByName(string name)
		{
			var mobileWebPage = await _mobileWebPageRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

			var output = new GetMobileWebPageForViewDto { MobileWebPage = ObjectMapper.Map<MobileWebPageDto>(mobileWebPage) };

			return output;
		}
	}
}