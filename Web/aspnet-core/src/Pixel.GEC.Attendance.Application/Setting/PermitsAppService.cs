

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
	[AbpAuthorize(AppPermissions.Pages_Permits)]
    public class PermitsAppService : AttendanceAppServiceBase, IPermitsAppService
    {
		 private readonly IRepository<Permit> _permitRepository;
        private readonly IRepository<TypesOfPermit> _typeOfPermitRepository;
        private readonly IPermitsExcelExporter _permitsExcelExporter;
		 

		  public PermitsAppService(IRepository<Permit> permitRepository, IRepository<TypesOfPermit> typeOfPermitRepository,
        IPermitsExcelExporter permitsExcelExporter) 
		  {
			_permitRepository = permitRepository;
			_permitsExcelExporter = permitsExcelExporter;
            _typeOfPermitRepository = typeOfPermitRepository;


          }

		 public async Task<PagedResultDto<GetPermitForViewDto>> GetAll(GetAllPermitsInput input)
         {
			
			var filteredPermits = _permitRepository.GetAll()
						
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter);

			var pagedAndFilteredPermits = filteredPermits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var permits = from o in pagedAndFilteredPermits
                         select new GetPermitForViewDto() {
							Permit = new PermitDto
							{
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                LateIn = o.LateIn,
                                EarlyOut = o.EarlyOut,
                                OffShift = o.OffShift,
                                FullDay = o.FullDay,
                                InOut = o.InOut,
                                ToleranceIn = o.ToleranceIn,
                                ToleranceOut = o.ToleranceOut,
                                MaxNumberPerDay = o.MaxNumberPerDay,
                                MaxNumberPerWeek = o.MaxNumberPerWeek,
                                MaxNumberPerMonth = o.MaxNumberPerMonth,
                                TotalHoursPerDay = o.TotalHoursPerDay,
                                TotalHoursPerWeek = o.TotalHoursPerWeek,
                                TotalHoursPerMonth = o.TotalHoursPerMonth,
                                IsDeducted=o.IsDeducted,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPermits.CountAsync();

            return new PagedResultDto<GetPermitForViewDto>(
                totalCount,
                await permits.ToListAsync()
            );
         }
		 
		 public async Task<GetPermitForViewDto> GetPermitForView(int id)
         {
            var permit = await _permitRepository.GetAsync(id);

            var output = new GetPermitForViewDto { Permit = ObjectMapper.Map<PermitDto>(permit) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Permits_Edit)]
		 public async Task<GetPermitForEditOutput> GetPermitForEdit(EntityDto input)
         {
            var permit = await _permitRepository.FirstOrDefaultAsync(p => p.Id == input.Id);

            var output = new GetPermitForEditOutput {Permit = ObjectMapper.Map<CreateOrEditPermitDto>(permit)};

            return output;
         }

        private List<AssignedTypesOfPermitDto> PopulateAssignedTypesOfPermit(Permit permit)
        {
            var allTypesOfPermit = _typeOfPermitRepository.GetAll();
            var permitTypes = new HashSet<int>(permit.PermitTypes.Select(p => p.PermitId));
            var assignedTypesOfPermit = new List<AssignedTypesOfPermitDto>();

            foreach (var typeOfPermit in allTypesOfPermit)
            {
                assignedTypesOfPermit.Add(new AssignedTypesOfPermitDto
                {
                    TypeOfPermitId = typeOfPermit.Id,
                    NameAr = typeOfPermit.NameAr,
                    Assigned = permitTypes.Contains(typeOfPermit.Id)
                });
            }

            return assignedTypesOfPermit;

        }

		 public async Task CreateOrEdit(CreateOrEditPermitDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Permits_Create)]
		 protected virtual async Task Create(CreateOrEditPermitDto input)
         {

            var permit = ObjectMapper.Map<Permit>(input);
            await _permitRepository.InsertAsync(permit);
         }

		 [AbpAuthorize(AppPermissions.Pages_Permits_Edit)]
		 protected virtual async Task Update(CreateOrEditPermitDto input)
         {
            var permit = await _permitRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, permit);
         }

		 [AbpAuthorize(AppPermissions.Pages_Permits_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _permitRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPermitsToExcel(GetAllPermitsForExcelInput input)
         {
			
			var filteredPermits = _permitRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DescriptionAr.Contains(input.Filter) || e.DescriptionEn.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionArFilter),  e => e.DescriptionAr == input.DescriptionArFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionEnFilter),  e => e.DescriptionEn == input.DescriptionEnFilter);

			var query = (from o in filteredPermits
                         select new GetPermitForViewDto() { 
							Permit = new PermitDto
							{
                                DescriptionAr = o.DescriptionAr,
                                DescriptionEn = o.DescriptionEn,
                                LateIn = o.LateIn,
                                EarlyOut = o.EarlyOut,
                                OffShift = o.OffShift,
                                FullDay = o.FullDay,
                                InOut = o.InOut,
                                ToleranceIn = o.ToleranceIn,
                                ToleranceOut = o.ToleranceOut,
                                MaxNumberPerDay = o.MaxNumberPerDay,
                                MaxNumberPerWeek = o.MaxNumberPerWeek,
                                MaxNumberPerMonth = o.MaxNumberPerMonth,
                                TotalHoursPerDay = o.TotalHoursPerDay,
                                TotalHoursPerWeek = o.TotalHoursPerWeek,
                                TotalHoursPerMonth = o.TotalHoursPerMonth,
                                Id = o.Id
							}
						 });


            var permitListDtos = await query.ToListAsync();

            return _permitsExcelExporter.ExportToFile(permitListDtos);
         }


    }
}