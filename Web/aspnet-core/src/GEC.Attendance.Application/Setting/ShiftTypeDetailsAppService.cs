using GEC.Attendance.Setting;


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
	[AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails)]
    public class ShiftTypeDetailsAppService : AttendanceAppServiceBase, IShiftTypeDetailsAppService
    {
		 private readonly IRepository<ShiftTypeDetail> _shiftTypeDetailRepository;
		 private readonly IShiftTypeDetailsExcelExporter _shiftTypeDetailsExcelExporter;
		 private readonly IRepository<ShiftType,int> _lookup_shiftTypeRepository;
		 

		  public ShiftTypeDetailsAppService(IRepository<ShiftTypeDetail> shiftTypeDetailRepository, IShiftTypeDetailsExcelExporter shiftTypeDetailsExcelExporter , IRepository<ShiftType, int> lookup_shiftTypeRepository) 
		  {
			_shiftTypeDetailRepository = shiftTypeDetailRepository;
			_shiftTypeDetailsExcelExporter = shiftTypeDetailsExcelExporter;
			_lookup_shiftTypeRepository = lookup_shiftTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetShiftTypeDetailForViewDto>> GetAll(GetAllShiftTypeDetailsInput input)
         {
			
			var filteredShiftTypeDetails = _shiftTypeDetailRepository.GetAll()
						.Include( e => e.ShiftTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftTypeDescriptionArFilter), e => e.ShiftTypeFk != null && e.ShiftTypeFk.DescriptionAr == input.ShiftTypeDescriptionArFilter);

			var pagedAndFilteredShiftTypeDetails = filteredShiftTypeDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shiftTypeDetails = from o in pagedAndFilteredShiftTypeDetails
                         join o1 in _lookup_shiftTypeRepository.GetAll() on o.ShiftTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetShiftTypeDetailForViewDto() {
							ShiftTypeDetail = new ShiftTypeDetailDto
							{
                                InTimeFirstScan = o.InTimeFirstScan,
                                InTimeLastScan = o.InTimeLastScan,
                                OutTimeFirstScan = o.OutTimeFirstScan,
                                OutTimeLastScan = o.OutTimeLastScan,
                                Id = o.Id
							},
                         	ShiftTypeDescriptionAr = s1 == null ? "" : s1.DescriptionAr.ToString()
						};

            var totalCount = await filteredShiftTypeDetails.CountAsync();

            return new PagedResultDto<GetShiftTypeDetailForViewDto>(
                totalCount,
                await shiftTypeDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetShiftTypeDetailForViewDto> GetShiftTypeDetailForView(int id)
         {
            var shiftTypeDetail = await _shiftTypeDetailRepository.GetAsync(id);

            var output = new GetShiftTypeDetailForViewDto { ShiftTypeDetail = ObjectMapper.Map<ShiftTypeDetailDto>(shiftTypeDetail) };

		    if (output.ShiftTypeDetail.ShiftTypeId != null)
            {
                var _lookupShiftType = await _lookup_shiftTypeRepository.FirstOrDefaultAsync((int)output.ShiftTypeDetail.ShiftTypeId);
                output.ShiftTypeDescriptionAr = _lookupShiftType.DescriptionAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails_Edit)]
		 public async Task<GetShiftTypeDetailForEditOutput> GetShiftTypeDetailForEdit(EntityDto input)
         {
            var shiftTypeDetail = await _shiftTypeDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftTypeDetailForEditOutput {ShiftTypeDetail = ObjectMapper.Map<CreateOrEditShiftTypeDetailDto>(shiftTypeDetail)};

		    if (output.ShiftTypeDetail.ShiftTypeId != null)
            {
                var _lookupShiftType = await _lookup_shiftTypeRepository.FirstOrDefaultAsync((int)output.ShiftTypeDetail.ShiftTypeId);
                output.ShiftTypeDescriptionAr = _lookupShiftType.DescriptionAr.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftTypeDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails_Create)]
		 protected virtual async Task Create(CreateOrEditShiftTypeDetailDto input)
         {
            var shiftTypeDetail = ObjectMapper.Map<ShiftTypeDetail>(input);

			

            await _shiftTypeDetailRepository.InsertAsync(shiftTypeDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftTypeDetailDto input)
         {
            var shiftTypeDetail = await _shiftTypeDetailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shiftTypeDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftTypeDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftTypeDetailsToExcel(GetAllShiftTypeDetailsForExcelInput input)
         {
			
			var filteredShiftTypeDetails = _shiftTypeDetailRepository.GetAll()
						.Include( e => e.ShiftTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftTypeDescriptionArFilter), e => e.ShiftTypeFk != null && e.ShiftTypeFk.DescriptionAr == input.ShiftTypeDescriptionArFilter);

			var query = (from o in filteredShiftTypeDetails
                         join o1 in _lookup_shiftTypeRepository.GetAll() on o.ShiftTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetShiftTypeDetailForViewDto() { 
							ShiftTypeDetail = new ShiftTypeDetailDto
							{
                                InTimeFirstScan = o.InTimeFirstScan,
                                InTimeLastScan = o.InTimeLastScan,
                                OutTimeFirstScan = o.OutTimeFirstScan,
                                OutTimeLastScan = o.OutTimeLastScan,
                                Id = o.Id
							},
                         	ShiftTypeDescriptionAr = s1 == null ? "" : s1.DescriptionAr.ToString()
						 });


            var shiftTypeDetailListDtos = await query.ToListAsync();

            return _shiftTypeDetailsExcelExporter.ExportToFile(shiftTypeDetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ShiftTypeDetails)]
         public async Task<PagedResultDto<ShiftTypeDetailShiftTypeLookupTableDto>> GetAllShiftTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DescriptionAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftTypeDetailShiftTypeLookupTableDto>();
			foreach(var shiftType in shiftTypeList){
				lookupTableDtoList.Add(new ShiftTypeDetailShiftTypeLookupTableDto
				{
					Id = shiftType.Id,
					DisplayName = shiftType.DescriptionAr?.ToString()
				});
			}

            return new PagedResultDto<ShiftTypeDetailShiftTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}