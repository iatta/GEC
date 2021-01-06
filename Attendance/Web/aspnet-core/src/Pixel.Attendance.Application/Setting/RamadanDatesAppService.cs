

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
	[AbpAuthorize(AppPermissions.Pages_RamadanDates)]
    public class RamadanDatesAppService : AttendanceAppServiceBase, IRamadanDatesAppService
    {
		 private readonly IRepository<RamadanDate> _ramadanDateRepository;
		 private readonly IRamadanDatesExcelExporter _ramadanDatesExcelExporter;
		 

		  public RamadanDatesAppService(IRepository<RamadanDate> ramadanDateRepository, IRamadanDatesExcelExporter ramadanDatesExcelExporter ) 
		  {
			_ramadanDateRepository = ramadanDateRepository;
			_ramadanDatesExcelExporter = ramadanDatesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRamadanDateForViewDto>> GetAll(GetAllRamadanDatesInput input)
         {
			
			var filteredRamadanDates = _ramadanDateRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Year.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.YearFilter),  e => e.Year == input.YearFilter)
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter);

			var pagedAndFilteredRamadanDates = filteredRamadanDates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var ramadanDates = from o in pagedAndFilteredRamadanDates
                         select new GetRamadanDateForViewDto() {
							RamadanDate = new RamadanDateDto
							{
                                Year = o.Year,
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredRamadanDates.CountAsync();

            return new PagedResultDto<GetRamadanDateForViewDto>(
                totalCount,
                await ramadanDates.ToListAsync()
            );
         }
		 
		 public async Task<GetRamadanDateForViewDto> GetRamadanDateForView(int id)
         {
            var ramadanDate = await _ramadanDateRepository.GetAsync(id);

            var output = new GetRamadanDateForViewDto { RamadanDate = ObjectMapper.Map<RamadanDateDto>(ramadanDate) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_RamadanDates_Edit)]
		 public async Task<GetRamadanDateForEditOutput> GetRamadanDateForEdit(EntityDto input)
         {
            var ramadanDate = await _ramadanDateRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRamadanDateForEditOutput {RamadanDate = ObjectMapper.Map<CreateOrEditRamadanDateDto>(ramadanDate)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRamadanDateDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_RamadanDates_Create)]
		 protected virtual async Task Create(CreateOrEditRamadanDateDto input)
         {
            var ramadanDate = ObjectMapper.Map<RamadanDate>(input);

			

            await _ramadanDateRepository.InsertAsync(ramadanDate);
         }

		 [AbpAuthorize(AppPermissions.Pages_RamadanDates_Edit)]
		 protected virtual async Task Update(CreateOrEditRamadanDateDto input)
         {
            var ramadanDate = await _ramadanDateRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, ramadanDate);
         }

		 [AbpAuthorize(AppPermissions.Pages_RamadanDates_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _ramadanDateRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetRamadanDatesToExcel(GetAllRamadanDatesForExcelInput input)
         {
			
			var filteredRamadanDates = _ramadanDateRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Year.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.YearFilter),  e => e.Year == input.YearFilter)
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter);

			var query = (from o in filteredRamadanDates
                         select new GetRamadanDateForViewDto() { 
							RamadanDate = new RamadanDateDto
							{
                                Year = o.Year,
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							}
						 });


            var ramadanDateListDtos = await query.ToListAsync();

            return _ramadanDatesExcelExporter.ExportToFile(ramadanDateListDtos);
         }


    }
}