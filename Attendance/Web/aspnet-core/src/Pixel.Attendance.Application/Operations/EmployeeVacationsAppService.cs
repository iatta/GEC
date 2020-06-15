using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Setting;


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
using Abp.UI;

namespace Pixel.Attendance.Operations
{
	[AbpAuthorize(AppPermissions.Pages_EmployeeVacations)]
    public class EmployeeVacationsAppService : AttendanceAppServiceBase, IEmployeeVacationsAppService
    {
		 private readonly IRepository<EmployeeVacation> _employeeVacationRepository;
        private readonly IRepository<LeaveType> _leaveTypeRepository;
        private readonly IEmployeeVacationsExcelExporter _employeeVacationsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<LeaveType,int> _lookup_leaveTypeRepository;
        private readonly UserManager _userManager;
		 

		  public EmployeeVacationsAppService(IRepository<EmployeeVacation> employeeVacationRepository, 
              IEmployeeVacationsExcelExporter employeeVacationsExcelExporter , 
              IRepository<User, long> lookup_userRepository, IRepository<LeaveType, int> lookup_leaveTypeRepository ,UserManager userManager, IRepository<LeaveType> leaveType) 
		  {
			_employeeVacationRepository = employeeVacationRepository;
			_employeeVacationsExcelExporter = employeeVacationsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		    _lookup_leaveTypeRepository = lookup_leaveTypeRepository;
            _userManager = userManager;
            _leaveTypeRepository = leaveType;


          }

		 public async Task<PagedResultDto<GetEmployeeVacationForViewDto>> GetAll(GetAllEmployeeVacationsInput input)
         {
			
			var filteredEmployeeVacations = _employeeVacationRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.LeaveTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveTypeNameArFilter), e => e.LeaveTypeFk != null && e.LeaveTypeFk.NameAr == input.LeaveTypeNameArFilter);

			var pagedAndFilteredEmployeeVacations = filteredEmployeeVacations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeVacations = from o in pagedAndFilteredEmployeeVacations
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_leaveTypeRepository.GetAll() on o.LeaveTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeVacationForViewDto() {
							EmployeeVacation = new EmployeeVacationDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Description = o.Description,
                                Status = o.Status,
                                Note = o.Note,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	LeaveTypeNameAr = s2 == null ? "" : s2.NameAr.ToString()
						};

            var totalCount = await filteredEmployeeVacations.CountAsync();

            return new PagedResultDto<GetEmployeeVacationForViewDto>(
                totalCount,
                await employeeVacations.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeVacationForViewDto> GetEmployeeVacationForView(int id)
         {
            var employeeVacation = await _employeeVacationRepository.GetAsync(id);

            var output = new GetEmployeeVacationForViewDto { EmployeeVacation = ObjectMapper.Map<EmployeeVacationDto>(employeeVacation) };

		    if (output.EmployeeVacation.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeVacation.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeeVacation.LeaveTypeId != null)
            {
                var _lookupLeaveType = await _lookup_leaveTypeRepository.FirstOrDefaultAsync((int)output.EmployeeVacation.LeaveTypeId);
                output.LeaveTypeNameAr = _lookupLeaveType.NameAr.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeVacations_Edit)]
		 public async Task<GetEmployeeVacationForEditOutput> GetEmployeeVacationForEdit(EntityDto input)
         {
            var employeeVacation = await _employeeVacationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeVacationForEditOutput {EmployeeVacation = ObjectMapper.Map<CreateOrEditEmployeeVacationDto>(employeeVacation)};

		    if (output.EmployeeVacation.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.EmployeeVacation.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.EmployeeVacation.LeaveTypeId != null)
            {
                var _lookupLeaveType = await _lookup_leaveTypeRepository.FirstOrDefaultAsync((int)output.EmployeeVacation.LeaveTypeId);
                output.LeaveTypeNameAr = _lookupLeaveType.NameAr.ToString();
            }
			
            return output;
         }

        public async Task CreateFromExcel(List<CreateOrEditEmployeeVacationDto> input)
        {
            try
            {
                using (var uom = UnitOfWorkManager.Begin())
                {
                    foreach (var item in input)
                    {
                        var leaveId = _leaveTypeRepository.FirstOrDefault(x => x.Code == item.LeaveCode).Id;
                        var userId = _userManager.Users.FirstOrDefault(x => x.Code == item.EmpCode).Id;
                        var itemToAdd = new CreateOrEditEmployeeVacationDto();
                        itemToAdd.Description = item.Description;
                        itemToAdd.FromDate = item.FromDate;
                        itemToAdd.ToDate = item.ToDate;
                        item.LeaveTypeId = leaveId;
                        item.UserId = userId;

                        var employeeVacation = ObjectMapper.Map<EmployeeVacation>(item);
                        await _employeeVacationRepository.InsertAsync(employeeVacation);
                    }
                    uom.Complete();
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("failed");
            }
        }
		 public async Task CreateOrEdit(CreateOrEditEmployeeVacationDto input)
         {
            if(input.Id == null)
            {

				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeVacations_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeVacationDto input)
         {
            var employeeVacation = ObjectMapper.Map<EmployeeVacation>(input);

			

            await _employeeVacationRepository.InsertAsync(employeeVacation);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeVacations_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeVacationDto input)
         {
            var employeeVacation = await _employeeVacationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeVacation);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeVacations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeVacationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeVacationsToExcel(GetAllEmployeeVacationsForExcelInput input)
         {
			
			var filteredEmployeeVacations = _employeeVacationRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.LeaveTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(input.MinFromDateFilter != null, e => e.FromDate >= input.MinFromDateFilter)
						.WhereIf(input.MaxFromDateFilter != null, e => e.FromDate <= input.MaxFromDateFilter)
						.WhereIf(input.MinToDateFilter != null, e => e.ToDate >= input.MinToDateFilter)
						.WhereIf(input.MaxToDateFilter != null, e => e.ToDate <= input.MaxToDateFilter)
						.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LeaveTypeNameArFilter), e => e.LeaveTypeFk != null && e.LeaveTypeFk.NameAr == input.LeaveTypeNameArFilter);

			var query = (from o in filteredEmployeeVacations
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_leaveTypeRepository.GetAll() on o.LeaveTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeVacationForViewDto() { 
							EmployeeVacation = new EmployeeVacationDto
							{
                                FromDate = o.FromDate,
                                ToDate = o.ToDate,
                                Description = o.Description,
                                Status = o.Status,
                                Note = o.Note,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	LeaveTypeNameAr = s2 == null ? "" : s2.NameAr.ToString()
						 });


            var employeeVacationListDtos = await query.ToListAsync();

            return _employeeVacationsExcelExporter.ExportToFile(employeeVacationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeVacations)]
         public async Task<PagedResultDto<EmployeeVacationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeVacationUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new EmployeeVacationUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeeVacationUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_EmployeeVacations)]
         public async Task<PagedResultDto<EmployeeVacationLeaveTypeLookupTableDto>> GetAllLeaveTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_leaveTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.NameAr.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var leaveTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeVacationLeaveTypeLookupTableDto>();
			foreach(var leaveType in leaveTypeList){
				lookupTableDtoList.Add(new EmployeeVacationLeaveTypeLookupTableDto
				{
					Id = leaveType.Id,
					DisplayName = leaveType.NameAr?.ToString()
				});
			}

            return new PagedResultDto<EmployeeVacationLeaveTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}