using Pixel.GEC.Attendance.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.GEC.Attendance.Operation.Exporting;
using Pixel.GEC.Attendance.Operation.Dtos;
using Pixel.GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.Operation
{
	[AbpAuthorize(AppPermissions.Pages_EmployeeAbsences)]
    public class EmployeeAbsencesAppService : AttendanceAppServiceBase, IEmployeeAbsencesAppService
    {
		 private readonly IRepository<EmployeeAbsence> _employeeAbsenceRepository;
		 private readonly IEmployeeAbsencesExcelExporter _employeeAbsencesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public EmployeeAbsencesAppService(IRepository<EmployeeAbsence> employeeAbsenceRepository, IEmployeeAbsencesExcelExporter employeeAbsencesExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_employeeAbsenceRepository = employeeAbsenceRepository;
			_employeeAbsencesExcelExporter = employeeAbsencesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeeAbsenceForViewDto>> GetAll(GetAllEmployeeAbsencesInput input)
         {
			
			var filteredEmployeeAbsences = _employeeAbsenceRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredEmployeeAbsences = filteredEmployeeAbsences
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeAbsences = from o in pagedAndFilteredEmployeeAbsences
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetEmployeeAbsenceForViewDto() {
							EmployeeAbsence = new EmployeeAbsenceDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredEmployeeAbsences.CountAsync();

            return new PagedResultDto<GetEmployeeAbsenceForViewDto>(
                totalCount,
                await employeeAbsences.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeAbsenceForViewDto> GetEmployeeAbsenceForView(int id)
         {
            var employeeAbsence = await _employeeAbsenceRepository.GetAsync(id);

            var output = new GetEmployeeAbsenceForViewDto { EmployeeAbsence = ObjectMapper.Map<EmployeeAbsenceDto>(employeeAbsence) };

		    if (output.EmployeeAbsence.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeAbsence.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeAbsences_Edit)]
		 public async Task<GetEmployeeAbsenceForEditOutput> GetEmployeeAbsenceForEdit(EntityDto input)
         {
            var employeeAbsence = await _employeeAbsenceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeAbsenceForEditOutput {EmployeeAbsence = ObjectMapper.Map<CreateOrEditEmployeeAbsenceDto>(employeeAbsence)};

		    if (output.EmployeeAbsence.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeAbsence.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeAbsenceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeAbsences_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeAbsenceDto input)
         {
            var employeeAbsence = ObjectMapper.Map<EmployeeAbsence>(input);

			

            await _employeeAbsenceRepository.InsertAsync(employeeAbsence);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeAbsences_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeAbsenceDto input)
         {
            var employeeAbsence = await _employeeAbsenceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeAbsence);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeAbsences_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeAbsenceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeAbsencesToExcel(GetAllEmployeeAbsencesForExcelInput input)
         {
			
			var filteredEmployeeAbsences = _employeeAbsenceRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredEmployeeAbsences
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetEmployeeAbsenceForViewDto() { 
							EmployeeAbsence = new EmployeeAbsenceDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var employeeAbsenceListDtos = await query.ToListAsync();

            return _employeeAbsencesExcelExporter.ExportToFile(employeeAbsenceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeAbsences)]
         public async Task<PagedResultDto<EmployeeAbsenceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeAbsenceUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new EmployeeAbsenceUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeeAbsenceUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}