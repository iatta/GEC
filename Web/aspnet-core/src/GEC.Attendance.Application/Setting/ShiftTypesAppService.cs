

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
	[AbpAuthorize(AppPermissions.Pages_ShiftTypes)]
    public class ShiftTypesAppService : AttendanceAppServiceBase, IShiftTypesAppService
    {
		 private readonly IRepository<ShiftType> _shiftTypeRepository;
        private readonly IRepository<ShiftTypeDetail> _shiftTypeDetailRepository;
        private readonly IShiftTypesExcelExporter _shiftTypesExcelExporter;
		 

		  public ShiftTypesAppService(IRepository<ShiftType> shiftTypeRepository, IRepository<ShiftTypeDetail> shiftTypeDetailRepository , IShiftTypesExcelExporter shiftTypesExcelExporter ) 
		  {
			_shiftTypeRepository = shiftTypeRepository;
            _shiftTypeDetailRepository = shiftTypeDetailRepository;
            _shiftTypesExcelExporter = shiftTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetShiftTypeForViewDto>> GetAll(GetAllShiftTypesInput input)
         {
			
			var filteredShiftTypes = _shiftTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DescriptionEn.Contains(input.Filter) || e.DescriptionAr.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter);

			var pagedAndFilteredShiftTypes = filteredShiftTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shiftTypes = from o in pagedAndFilteredShiftTypes
                         select new GetShiftTypeForViewDto() {
							ShiftType = new ShiftTypeDto
							{
                                DescriptionEn = o.DescriptionEn,
                                DescriptionAr = o.DescriptionAr,
                                NumberOfDuties = o.NumberOfDuties,
                                InScan = o.InScan,
                                OutScan = o.OutScan,
                                CrossDay = o.CrossDay,
                                AlwaysAttend = o.AlwaysAttend,
                                Open = o.Open,
                                MaxBoundryTime = o.MaxBoundryTime,
                                Id = o.Id
							}
						};

            var totalCount = await filteredShiftTypes.CountAsync();

            return new PagedResultDto<GetShiftTypeForViewDto>(
                totalCount,
                await shiftTypes.ToListAsync()
            );
         }

        public async Task<List<GetShiftTypeForViewDto>> GetAllFlat()
        {

            var filteredShiftTypes = _shiftTypeRepository.GetAll();

          
            var shiftTypes = from o in filteredShiftTypes
                             select new GetShiftTypeForViewDto()
                             {
                                 ShiftType = new ShiftTypeDto
                                 {
                                     DescriptionEn = o.DescriptionEn,
                                     DescriptionAr = o.DescriptionAr,
                                     NumberOfDuties = o.NumberOfDuties,
                                     InScan = o.InScan,
                                     OutScan = o.OutScan,
                                     CrossDay = o.CrossDay,
                                     AlwaysAttend = o.AlwaysAttend,
                                     Open = o.Open,
                                     MaxBoundryTime = o.MaxBoundryTime,
                                     Id = o.Id
                                 }
                             };

            

            return new List<GetShiftTypeForViewDto>(await shiftTypes.ToListAsync());
        }

        public async Task<GetShiftTypeForViewDto> GetShiftTypeForView(int id)
         {
            var shiftType = await _shiftTypeRepository.GetAsync(id);

            var output = new GetShiftTypeForViewDto { ShiftType = ObjectMapper.Map<ShiftTypeDto>(shiftType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ShiftTypes_Edit)]
		 public async Task<GetShiftTypeForEditOutput> GetShiftTypeForEdit(EntityDto input)
         {
            var shiftType = await _shiftTypeRepository.GetAllIncluding(a => a.ShiftTypeDetails).FirstOrDefaultAsync(x => x.Id == input.Id);
           
		    var output = new GetShiftTypeForEditOutput {ShiftType = ObjectMapper.Map<CreateOrEditShiftTypeDto>(shiftType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypes_Create)]
		 protected virtual async Task Create(CreateOrEditShiftTypeDto input)
         {
            var shiftType = ObjectMapper.Map<ShiftType>(input);

            shiftType.ShiftTypeDetails = new List<ShiftTypeDetail>();

            if (input.ShiftTypeDetails.Count > 0)
            {
                foreach (var item in input.ShiftTypeDetails)
                {
                    shiftType.ShiftTypeDetails.Add(ObjectMapper.Map<ShiftTypeDetail>(item));
                }
                
            }
			

            await _shiftTypeRepository.InsertAsync(shiftType);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftTypeDto input)
         {
               //temp solution need to adjustment 

                var shiftType = await _shiftTypeRepository.GetAllIncluding(m => m.ShiftTypeDetails).FirstOrDefaultAsync(x => x.Id == (int)input.Id);

                var oldShiftTypeDetails = new HashSet<ShiftTypeDetail>(shiftType.ShiftTypeDetails.ToList());
                var newShifTypeDetails = new HashSet<ShiftTypeDetailDto>(input.ShiftTypeDetails.ToList());

                foreach (var detail in oldShiftTypeDetails)
                {
                    if (!newShifTypeDetails.Any(x => x.Id  == detail.Id))
                    {
                            shiftType.ShiftTypeDetails.Remove(detail);
                    }
                    else
                    {
                        var inputDetail = newShifTypeDetails.Where(x => x.Id == detail.Id).FirstOrDefault();
                        detail.InTimeFirstScan = inputDetail.InTimeFirstScan;
                        detail.InTimeLastScan = inputDetail.InTimeLastScan;
                        detail.OutTimeFirstScan = inputDetail.OutTimeFirstScan;
                        detail.OutTimeLastScan = inputDetail.OutTimeLastScan;
                    }

                }

                foreach (var item in newShifTypeDetails)
                {
                    if (item.Id == 0)
                    {
                        shiftType.ShiftTypeDetails.Add(ObjectMapper.Map<ShiftTypeDetail>(item));
                    }
                }
           
                

                shiftType.DescriptionAr = input.DescriptionAr;
                shiftType.DescriptionEn = input.DescriptionEn;
                shiftType.InScan = input.InScan;
                shiftType.OutScan = input.OutScan;
                shiftType.CrossDay = input.CrossDay;
                shiftType.AlwaysAttend = input.AlwaysAttend;
                shiftType.Open = input.Open;
                shiftType.MaxBoundryTime = input.MaxBoundryTime;
                shiftType.NumberOfDuties = input.ShiftTypeDetails.Count();


            await _shiftTypeRepository.UpdateAsync(shiftType);

          
        }

		 [AbpAuthorize(AppPermissions.Pages_ShiftTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftTypesToExcel(GetAllShiftTypesForExcelInput input)
         {
			
			var filteredShiftTypes = _shiftTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DescriptionEn.Contains(input.Filter) || e.DescriptionAr.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter);

			var query = (from o in filteredShiftTypes
                         select new GetShiftTypeForViewDto() { 
							ShiftType = new ShiftTypeDto
							{
                                DescriptionEn = o.DescriptionEn,
                                DescriptionAr = o.DescriptionAr,
                                NumberOfDuties = o.NumberOfDuties,
                                InScan = o.InScan,
                                OutScan = o.OutScan,
                                CrossDay = o.CrossDay,
                                AlwaysAttend = o.AlwaysAttend,
                                Open = o.Open,
                                MaxBoundryTime = o.MaxBoundryTime,
                                Id = o.Id
							}
						 });


            var shiftTypeListDtos = await query.ToListAsync();

            return _shiftTypesExcelExporter.ExportToFile(shiftTypeListDtos);
         }


    }
}