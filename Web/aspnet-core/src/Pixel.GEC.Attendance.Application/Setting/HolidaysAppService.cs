

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
	[AbpAuthorize(AppPermissions.Pages_Holidays)]
    public class HolidaysAppService : AttendanceAppServiceBase, IHolidaysAppService
    {
		 private readonly IRepository<Holiday> _HolidayRepository;
		 private readonly IHolidaysExcelExporter _holidaysExcelExporter;
		 

		  public HolidaysAppService(IRepository<Holiday> HolidayRepository, IHolidaysExcelExporter holidaysExcelExporter ) 
		  {
			_HolidayRepository = HolidayRepository;
			_holidaysExcelExporter = holidaysExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetHolidayForViewDto>> GetAll(GetAllHolidaysInput input)
         {
			
			var filteredholidays = _HolidayRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter);

			var pagedAndFilteredholidays = filteredholidays
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var holidays = from o in pagedAndFilteredholidays
                         select new GetHolidayForViewDto() {
							Holiday = new HolidayDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredholidays.CountAsync();

            var output =  new PagedResultDto<GetHolidayForViewDto>(
                totalCount,
                await holidays.ToListAsync()
            );  

            return output;
         }
		 
		 public async Task<GetHolidayForViewDto> GetHolidayForView(int id)
         {
            var Holiday = await _HolidayRepository.GetAsync(id);

            var output = new GetHolidayForViewDto { Holiday = ObjectMapper.Map<HolidayDto>(Holiday) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Holidays_Edit)]
		 public async Task<GetHolidayForEditOutput> GetHolidayForEdit(EntityDto input)
         {
            var Holiday = await _HolidayRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHolidayForEditOutput {Holiday = ObjectMapper.Map<CreateOrEditHolidayDto>(Holiday)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHolidayDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Holidays_Create)]
		 protected virtual async Task Create(CreateOrEditHolidayDto input)
         {
            var Holiday = ObjectMapper.Map<Holiday>(input);

			

            await _HolidayRepository.InsertAsync(Holiday);
         }

		 [AbpAuthorize(AppPermissions.Pages_Holidays_Edit)]
		 protected virtual async Task Update(CreateOrEditHolidayDto input)
         {
            var Holiday = await _HolidayRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, Holiday);
         }

		 [AbpAuthorize(AppPermissions.Pages_Holidays_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _HolidayRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHolidaysToExcel(GetAllHolidaysForExcelInput input)
         {
			
			var filteredholidays = _HolidayRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter);

			var query = (from o in filteredholidays
                         select new GetHolidayForViewDto() { 
							Holiday = new HolidayDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Id = o.Id
							}
						 });


            var HolidayListDtos = await query.ToListAsync();

            return _holidaysExcelExporter.ExportToFile(HolidayListDtos);
         }


    }
}