

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.Attendance.Operations.Exporting;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_Beacons)]
    public class BeaconsAppService : AttendanceAppServiceBase, IBeaconsAppService
    {
		 private readonly IRepository<Beacon> _beaconRepository;
		 private readonly IBeaconsExcelExporter _beaconsExcelExporter;
		 

		  public BeaconsAppService(IRepository<Beacon> beaconRepository, IBeaconsExcelExporter beaconsExcelExporter ) 
		  {
			_beaconRepository = beaconRepository;
			_beaconsExcelExporter = beaconsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetBeaconForViewDto>> GetAll(GetAllBeaconsInput input)
         {
			
			var filteredBeacons = _beaconRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Uid.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UidFilter),  e => e.Uid == input.UidFilter)
						.WhereIf(input.MinMinorFilter != null, e => e.Minor >= input.MinMinorFilter)
						.WhereIf(input.MaxMinorFilter != null, e => e.Minor <= input.MaxMinorFilter)
						.WhereIf(input.MinMajorFilter != null, e => e.Major >= input.MinMajorFilter)
						.WhereIf(input.MaxMajorFilter != null, e => e.Major <= input.MaxMajorFilter);

			var pagedAndFilteredBeacons = filteredBeacons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var beacons = from o in pagedAndFilteredBeacons
                         select new GetBeaconForViewDto() {
							Beacon = new BeaconDto
							{
                                Name = o.Name,
                                Uid = o.Uid,
                                Minor = o.Minor,
                                Major = o.Major,
                                Id = o.Id
							}
						};

            var totalCount = await filteredBeacons.CountAsync();

            return new PagedResultDto<GetBeaconForViewDto>(
                totalCount,
                await beacons.ToListAsync()
            );
         }
		 
		 public async Task<GetBeaconForViewDto> GetBeaconForView(int id)
         {
            var beacon = await _beaconRepository.GetAsync(id);

            var output = new GetBeaconForViewDto { Beacon = ObjectMapper.Map<BeaconDto>(beacon) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Beacons_Edit)]
		 public async Task<GetBeaconForEditOutput> GetBeaconForEdit(EntityDto input)
         {
            var beacon = await _beaconRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBeaconForEditOutput {Beacon = ObjectMapper.Map<CreateOrEditBeaconDto>(beacon)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBeaconDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Beacons_Create)]
		 protected virtual async Task Create(CreateOrEditBeaconDto input)
         {
            var beacon = ObjectMapper.Map<Beacon>(input);

			

            await _beaconRepository.InsertAsync(beacon);
         }

		 [AbpAuthorize(AppPermissions.Pages_Beacons_Edit)]
		 protected virtual async Task Update(CreateOrEditBeaconDto input)
         {
            var beacon = await _beaconRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, beacon);
         }

		 [AbpAuthorize(AppPermissions.Pages_Beacons_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _beaconRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBeaconsToExcel(GetAllBeaconsForExcelInput input)
         {
			
			var filteredBeacons = _beaconRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Uid.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UidFilter),  e => e.Uid == input.UidFilter)
						.WhereIf(input.MinMinorFilter != null, e => e.Minor >= input.MinMinorFilter)
						.WhereIf(input.MaxMinorFilter != null, e => e.Minor <= input.MaxMinorFilter)
						.WhereIf(input.MinMajorFilter != null, e => e.Major >= input.MinMajorFilter)
						.WhereIf(input.MaxMajorFilter != null, e => e.Major <= input.MaxMajorFilter);

			var query = (from o in filteredBeacons
                         select new GetBeaconForViewDto() { 
							Beacon = new BeaconDto
							{
                                Name = o.Name,
                                Uid = o.Uid,
                                Minor = o.Minor,
                                Major = o.Major,
                                Id = o.Id
							}
						 });


            var beaconListDtos = await query.ToListAsync();

            return _beaconsExcelExporter.ExportToFile(beaconListDtos);
         }


    }
}