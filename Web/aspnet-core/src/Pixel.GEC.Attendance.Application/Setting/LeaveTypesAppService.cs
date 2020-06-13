

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
	[AbpAuthorize(AppPermissions.Pages_LeaveTypes)]
    public class LeaveTypesAppService : AttendanceAppServiceBase, ILeaveTypesAppService
    {
		 private readonly IRepository<LeaveType> _leaveTypeRepository;
		 private readonly ILeaveTypesExcelExporter _leaveTypesExcelExporter;
		 

		  public LeaveTypesAppService(IRepository<LeaveType> leaveTypeRepository, ILeaveTypesExcelExporter leaveTypesExcelExporter ) 
		  {
			_leaveTypeRepository = leaveTypeRepository;
			_leaveTypesExcelExporter = leaveTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLeaveTypeForViewDto>> GetAll(GetAllLeaveTypesInput input)
         {
			
			var filteredLeaveTypes = _leaveTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var pagedAndFilteredLeaveTypes = filteredLeaveTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var leaveTypes = from o in pagedAndFilteredLeaveTypes
                         select new GetLeaveTypeForViewDto() {
							LeaveType = new LeaveTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Code = o.Code,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLeaveTypes.CountAsync();

            return new PagedResultDto<GetLeaveTypeForViewDto>(
                totalCount,
                await leaveTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetLeaveTypeForViewDto> GetLeaveTypeForView(int id)
         {
            var leaveType = await _leaveTypeRepository.GetAsync(id);

            var output = new GetLeaveTypeForViewDto { LeaveType = ObjectMapper.Map<LeaveTypeDto>(leaveType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LeaveTypes_Edit)]
		 public async Task<GetLeaveTypeForEditOutput> GetLeaveTypeForEdit(EntityDto input)
         {
            var leaveType = await _leaveTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLeaveTypeForEditOutput {LeaveType = ObjectMapper.Map<CreateOrEditLeaveTypeDto>(leaveType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLeaveTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveTypes_Create)]
		 protected virtual async Task Create(CreateOrEditLeaveTypeDto input)
         {
            var leaveType = ObjectMapper.Map<LeaveType>(input);

			

            await _leaveTypeRepository.InsertAsync(leaveType);
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditLeaveTypeDto input)
         {
            var leaveType = await _leaveTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, leaveType);
         }

		 [AbpAuthorize(AppPermissions.Pages_LeaveTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _leaveTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLeaveTypesToExcel(GetAllLeaveTypesForExcelInput input)
         {
			
			var filteredLeaveTypes = _leaveTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter),  e => e.NameAr == input.NameArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter),  e => e.NameEn == input.NameEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var query = (from o in filteredLeaveTypes
                         select new GetLeaveTypeForViewDto() { 
							LeaveType = new LeaveTypeDto
							{
                                NameAr = o.NameAr,
                                NameEn = o.NameEn,
                                Code = o.Code,
                                Id = o.Id
							}
						 });


            var leaveTypeListDtos = await query.ToListAsync();

            return _leaveTypesExcelExporter.ExportToFile(leaveTypeListDtos);
         }


    }
}