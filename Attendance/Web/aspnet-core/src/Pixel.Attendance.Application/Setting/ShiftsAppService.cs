
using Pixel.Attendance.Enums;
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
	[AbpAuthorize(AppPermissions.Pages_Shifts)]
    public class ShiftsAppService : AttendanceAppServiceBase, IShiftsAppService
    {
		 private readonly IRepository<Shift> _shiftRepository;
		 private readonly IShiftsExcelExporter _shiftsExcelExporter;
		 

		  public ShiftsAppService(IRepository<Shift> shiftRepository, IShiftsExcelExporter shiftsExcelExporter ) 
		  {
			_shiftRepository = shiftRepository;
			_shiftsExcelExporter = shiftsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetShiftForViewDto>> GetAll(GetAllShiftsInput input)
         {
			
			var filteredShifts = _shiftRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var pagedAndFilteredShifts = filteredShifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shifts = from o in pagedAndFilteredShifts
                         select new GetShiftForViewDto() {
							Shift = new ShiftDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Code = o.Code,
                                TimeIn = o.TimeIn,
                                TimeOut = o.TimeOut,
                                IsFlexible = o.IsFlexible,
                                IsDayRestCalculated = o.IsDayRestCalculated,
                                IsOneFingerprint = o.IsOneFingerprint,
                                IsInOutWithoutClculateHours = o.IsInOutWithoutClculateHours,
                                IsTwoFingerprint = o.IsTwoFingerprint,
                                TimeInRamadan = o.TimeInRamadan,
                                TimeOutRamadan = o.TimeOutRamadan,
                                TotalHoursPerDay = o.TotalHoursPerDay,
                                TotalLateMinutesPerMonth = o.TotalLateMinutesPerMonth,
                                TotalHoursPerDayRamadan = o.TotalHoursPerDayRamadan,
                                TotalLateMinutesPerMonthRamadan = o.TotalLateMinutesPerMonthRamadan,
                                Id = o.Id,
                            }
						};

            var totalCount = await filteredShifts.CountAsync();

            return new PagedResultDto<GetShiftForViewDto>(
                totalCount,
                await shifts.ToListAsync()
            );
         }

        public async Task<List<GetShiftForViewDto>> GetAllFlat()
        {

            var filteredShifts = _shiftRepository.GetAll();
            
            var shifts = from o in filteredShifts
                         select new GetShiftForViewDto()
                         {
                             Shift = new ShiftDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 Code = o.Code,
                                 TimeIn = o.TimeIn,
                                 TimeOut = o.TimeOut,
                                 EarlyIn = o.EarlyIn,
                                 LateIn = o.LateIn,
                                 EarlyOut = o.EarlyOut,
                                 LateOut = o.LateOut,
                                 TimeInRangeFrom = o.TimeInRangeFrom,
                                 TimeInRangeTo = o.TimeInRangeTo,
                                 TimeOutRangeFrom = o.TimeOutRangeFrom,
                                 TimeOutRangeTo = o.TimeOutRangeTo,
                                 Id = o.Id,
                                 IsOverTimeAllowed = o.IsOverTimeAllowed,
                                 ShiftType= ObjectMapper.Map<ShiftTypeEnumDto>(o.ShiftType),
                                 DeductType = o.DeductType
                             }
                         };

            

            return new List<GetShiftForViewDto>(await shifts.ToListAsync());
        }

        public async Task<GetShiftForViewDto> GetShiftForView(int id)
         {
            var shift = await _shiftRepository.GetAsync(id);

            var output = new GetShiftForViewDto { Shift = ObjectMapper.Map<ShiftDto>(shift) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 public async Task<GetShiftForEditOutput> GetShiftForEdit(EntityDto input)
         {
            var shift = await _shiftRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftForEditOutput {Shift = ObjectMapper.Map<CreateOrEditShiftDto>(shift)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Create)]
		 protected virtual async Task Create(CreateOrEditShiftDto input)
         {
            var shift = ObjectMapper.Map<Shift>(input);

			

            await _shiftRepository.InsertAsync(shift);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftDto input)
         {
            var shift = await _shiftRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shift);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input)
         {
			
			var filteredShifts = _shiftRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var query = (from o in filteredShifts
                         select new GetShiftForViewDto() { 
							Shift = new ShiftDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Code = o.Code,
                                TimeIn = o.TimeIn,
                                TimeOut = o.TimeOut,
                                EarlyIn = o.EarlyIn,
                                LateIn = o.LateIn,
                                EarlyOut = o.EarlyOut,
                                LateOut = o.LateOut,
                                TimeInRangeFrom = o.TimeInRangeFrom,
                                TimeInRangeTo = o.TimeInRangeTo,
                                TimeOutRangeFrom = o.TimeOutRangeFrom,
                                TimeOutRangeTo = o.TimeOutRangeTo,
                                Id = o.Id,
                                IsOverTimeAllowed = o.IsOverTimeAllowed,
                                ShiftType = ObjectMapper.Map<ShiftTypeEnumDto>(o.ShiftType),
                                DeductType = o.DeductType
                            }
						 });


            var shiftListDtos = await query.ToListAsync();

            return _shiftsExcelExporter.ExportToFile(shiftListDtos);
         }


    }
}