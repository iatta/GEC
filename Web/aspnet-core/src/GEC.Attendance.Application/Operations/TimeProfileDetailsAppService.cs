using GEC.Attendance.Operations;
using GEC.Attendance.Setting;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using GEC.Attendance.Operations.Exporting;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GEC.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_TimeProfileDetails)]
    public class TimeProfileDetailsAppService : AttendanceAppServiceBase, ITimeProfileDetailsAppService
    {
		 private readonly IRepository<TimeProfileDetail> _timeProfileDetailRepository;
		 private readonly ITimeProfileDetailsExcelExporter _timeProfileDetailsExcelExporter;
		 private readonly IRepository<TimeProfile,int> _lookup_timeProfileRepository;
		 private readonly IRepository<Shift,int> _lookup_shiftRepository;
		 

		  public TimeProfileDetailsAppService(IRepository<TimeProfileDetail> timeProfileDetailRepository, ITimeProfileDetailsExcelExporter timeProfileDetailsExcelExporter , IRepository<TimeProfile, int> lookup_timeProfileRepository, IRepository<Shift, int> lookup_shiftRepository) 
		  {
			_timeProfileDetailRepository = timeProfileDetailRepository;
			_timeProfileDetailsExcelExporter = timeProfileDetailsExcelExporter;
			_lookup_timeProfileRepository = lookup_timeProfileRepository;
		_lookup_shiftRepository = lookup_shiftRepository;
		
		  }

		 public async Task<PagedResultDto<GetTimeProfileDetailForViewDto>> GetAll(GetAllTimeProfileDetailsInput input)
         {
			
			var filteredTimeProfileDetails = _timeProfileDetailRepository.GetAll()
						.Include( e => e.TimeProfileFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.TimeProfileDescriptionArFilter), e => e.TimeProfileFk != null && e.TimeProfileFk.DescriptionAr == input.TimeProfileDescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameArFilter), e => e.ShiftFk != null && e.ShiftFk.NameAr == input.ShiftNameArFilter);

			var pagedAndFilteredTimeProfileDetails = filteredTimeProfileDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var timeProfileDetails = from o in pagedAndFilteredTimeProfileDetails
                         join o1 in _lookup_timeProfileRepository.GetAll() on o.TimeProfileId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTimeProfileDetailForViewDto() {
							TimeProfileDetail = new TimeProfileDetailDto
							{
                                Id = o.Id
							},
                         	TimeProfileDescriptionAr = s1 == null ? "" : s1.DescriptionAr.ToString(),
                         	ShiftNameAr = s2 == null ? "" : s2.NameAr.ToString()
						};

            var totalCount = await filteredTimeProfileDetails.CountAsync();

            return new PagedResultDto<GetTimeProfileDetailForViewDto>(
                totalCount,
                await timeProfileDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetTimeProfileDetailForViewDto> GetTimeProfileDetailForView(int id)
         {
            var timeProfileDetail = await _timeProfileDetailRepository.GetAsync(id);

            var output = new GetTimeProfileDetailForViewDto { TimeProfileDetail = ObjectMapper.Map<TimeProfileDetailDto>(timeProfileDetail) };

		    if (output.TimeProfileDetail.TimeProfileId != null)
            {
                var _lookupTimeProfile = await _lookup_timeProfileRepository.FirstOrDefaultAsync((int)output.TimeProfileDetail.TimeProfileId);
                output.TimeProfileDescriptionAr = _lookupTimeProfile.DescriptionAr.ToString();
            }

		    if (output.TimeProfileDetail.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.TimeProfileDetail.ShiftId);
                output.ShiftNameAr = _lookupShift.NameAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TimeProfileDetails_Edit)]
		 public async Task<GetTimeProfileDetailForEditOutput> GetTimeProfileDetailForEdit(EntityDto input)
         {
            var timeProfileDetail = await _timeProfileDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTimeProfileDetailForEditOutput {TimeProfileDetail = ObjectMapper.Map<CreateOrEditTimeProfileDetailDto>(timeProfileDetail)};

		    if (output.TimeProfileDetail.TimeProfileId != null)
            {
                var _lookupTimeProfile = await _lookup_timeProfileRepository.FirstOrDefaultAsync((int)output.TimeProfileDetail.TimeProfileId);
                output.TimeProfileDescriptionAr = _lookupTimeProfile.DescriptionAr.ToString();
            }

		    if (output.TimeProfileDetail.ShiftId != null)
            {
                var _lookupShift = await _lookup_shiftRepository.FirstOrDefaultAsync((int)output.TimeProfileDetail.ShiftId);
                output.ShiftNameAr = _lookupShift.NameAr.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTimeProfileDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfileDetails_Create)]
		 protected virtual async Task Create(CreateOrEditTimeProfileDetailDto input)
         {
            var timeProfileDetail = ObjectMapper.Map<TimeProfileDetail>(input);

			

            await _timeProfileDetailRepository.InsertAsync(timeProfileDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfileDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditTimeProfileDetailDto input)
         {
            var timeProfileDetail = await _timeProfileDetailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, timeProfileDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_TimeProfileDetails_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _timeProfileDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTimeProfileDetailsToExcel(GetAllTimeProfileDetailsForExcelInput input)
         {
			
			var filteredTimeProfileDetails = _timeProfileDetailRepository.GetAll()
						.Include( e => e.TimeProfileFk)
						.Include( e => e.ShiftFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.TimeProfileDescriptionArFilter), e => e.TimeProfileFk != null && e.TimeProfileFk.DescriptionAr == input.TimeProfileDescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftNameArFilter), e => e.ShiftFk != null && e.ShiftFk.NameAr == input.ShiftNameArFilter);

			var query = (from o in filteredTimeProfileDetails
                         join o1 in _lookup_timeProfileRepository.GetAll() on o.TimeProfileId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTimeProfileDetailForViewDto() { 
							TimeProfileDetail = new TimeProfileDetailDto
							{
                                Id = o.Id
							},
                         	TimeProfileDescriptionAr = s1 == null ? "" : s1.DescriptionAr.ToString(),
                         	ShiftNameAr = s2 == null ? "" : s2.NameAr.ToString()
						 });


            var timeProfileDetailListDtos = await query.ToListAsync();

            return _timeProfileDetailsExcelExporter.ExportToFile(timeProfileDetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TimeProfileDetails)]
         public async Task<PagedResultDto<TimeProfileDetailTimeProfileLookupTableDto>> GetAllTimeProfileForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_timeProfileRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DescriptionAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var timeProfileList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TimeProfileDetailTimeProfileLookupTableDto>();
			foreach(var timeProfile in timeProfileList){
				lookupTableDtoList.Add(new TimeProfileDetailTimeProfileLookupTableDto
				{
					Id = timeProfile.Id,
					DisplayName = timeProfile.DescriptionAr?.ToString()
				});
			}

            return new PagedResultDto<TimeProfileDetailTimeProfileLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TimeProfileDetails)]
         public async Task<PagedResultDto<TimeProfileDetailShiftLookupTableDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TimeProfileDetailShiftLookupTableDto>();
			foreach(var shift in shiftList){
				lookupTableDtoList.Add(new TimeProfileDetailShiftLookupTableDto
				{
					Id = shift.Id,
					DisplayName = shift.NameAr?.ToString()
				});
			}

            return new PagedResultDto<TimeProfileDetailShiftLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}