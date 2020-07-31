

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
using Pixel.Attendance.Authorization.Users;
using Abp.Organizations;
using Pixel.Attendance.Extended;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Data.SqlClient;
using Pixel.Attendance.Setting;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using PayPalCheckoutSdk.Orders;
using NUglify.Helpers;
using Abp.Collections.Extensions;

namespace Pixel.Attendance.Operations
{
    [AbpAuthorize(AppPermissions.Pages_ManualTransactions)]
    public class TransactionsAppService : AttendanceAppServiceBase, ITransactionsAppService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<UserShift> _UserShiftRepository;
        private readonly IRepository<OrganizationUnitExtended,long> _organizationUnitRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly ITransactionsExcelExporter _transactionsExcelExporter;
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Machine, int> _lookup_machineRepository;
        private readonly IRepository<EmployeeVacation> _employeeVacation;
        private readonly IRepository<UserTimeSheetApprove> _userTimeSheetApproveRepository;
        


        public TransactionsAppService(IRepository<UserTimeSheetApprove> userTimeSheetApproveRepository  ,IRepository<EmployeeVacation> employeeVacation , IRepository<Shift> shiftRepository ,IRepository<UserShift> userShiftRepository, IRepository<OrganizationUnitExtended, long>  organizationUnit,IRepository<Transaction> transactionRepository, IRepository<Project> projectRepository, ITransactionsExcelExporter transactionsExcelExporter, UserManager userManager, IRepository<User, long> lookup_userRepository, IRepository<Machine, int> lookup_machineRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionsExcelExporter = transactionsExcelExporter;
            _userManager = userManager;
            _lookup_userRepository = lookup_userRepository;
            _projectRepository = projectRepository;
            _organizationUnitRepository = organizationUnit;
            _UserShiftRepository = userShiftRepository;
            _shiftRepository = shiftRepository;
            _lookup_machineRepository = lookup_machineRepository;
            _employeeVacation = employeeVacation;
            _userTimeSheetApproveRepository = userTimeSheetApproveRepository;

        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input)
        {

            var filteredTransactions = _transactionRepository.GetAll()
                        .Where(x => x.Manual == 1)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Time.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Remark.Contains(input.Filter))
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter);
                        
                        

            var pagedAndFilteredTransactions = filteredTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var transactions = from o in pagedAndFilteredTransactions
                               join o1 in _lookup_userRepository.GetAll() on o.Pin equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()
                               
                               join o2 in _lookup_machineRepository.GetAll() on o.MachineId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()
                               select new GetTransactionForViewDto()
                                {
                                    Transaction = new TransactionDto
                                    {
                                        TransType = o.TransType,
                                        KeyType = o.KeyType,
                                        CreationTime = o.CreationTime,
                                        Transaction_Date = o.Transaction_Date,
                                        Pin = o.Pin,
                                        Time = o.Time,
                                        Id = o.Id,
                                        ProjectManagerApprove=o.ProjectManagerApprove,
                                        UnitManagerApprove=o.UnitManagerApprove
                                    },
                                    UserName = s1 == null ? "" : s1.Name.ToString(),
                                    MachineNameEn = s2 == null ? "" : s2.NameEn.ToString(),
                                    MachineId = s2.Id,

                               };

            var totalCount = await filteredTransactions.CountAsync();

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
                await transactions.ToListAsync()
            );
        }

        public async Task<GetTransactionForViewDto> GetTransactionForView(int id)
        {
            var transaction = await _transactionRepository.GetAsync(id);

            var output = new GetTransactionForViewDto { Transaction = ObjectMapper.Map<TransactionDto>(transaction) };
            if (output.Transaction.Pin > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Transaction.Pin);
                output.UserName = _lookupUser.Name.ToString();
            }


            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
        public async Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto input)
        {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionForEditOutput { Transaction = ObjectMapper.Map<CreateOrEditTransactionDto>(transaction) };

            if (output.Transaction.Pin  > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Transaction.Pin);
                output.UserName = _lookupUser.Name.ToString();
                var _lookupMachine = await _lookup_machineRepository.FirstOrDefaultAsync(x => x.Id == output.Transaction.MachineId);
                output.MachineNameEn = _lookupMachine.NameEn.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTransactionDto input)
        {
            
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Create)]
        protected virtual async Task Create(CreateOrEditTransactionDto input)
        {
            input.Manual = 1;
            input.ImportDate = new DateTime();

            var transaction = ObjectMapper.Map<Transaction>(input);
            await _transactionRepository.InsertAsync(transaction);
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Edit)]
        protected virtual async Task Update(CreateOrEditTransactionDto input)
        {
            input.Manual = 1;
            input.ImportDate = new DateTime();


            var transaction = await _transactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transaction);
        }

        [AbpAuthorize(AppPermissions.Pages_ManualTransactions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _transactionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTransactionsToExcel(GetAllTransactionsForExcelInput input)
        {

            var filteredTransactions = _transactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Time.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Remark.Contains(input.Filter))
                        .WhereIf(input.MinTransTypeFilter != null, e => e.TransType >= input.MinTransTypeFilter)
                        .WhereIf(input.MaxTransTypeFilter != null, e => e.TransType <= input.MaxTransTypeFilter);

            var query = (from o in filteredTransactions
                         select new GetTransactionForViewDto()
                         {
                             Transaction = new TransactionDto
                             {
                                 TransType = o.TransType,
                                 Id = o.Id
                             }
                         });


            var transactionListDtos = await query.ToListAsync();

            return _transactionsExcelExporter.ExportToFile(transactionListDtos);
        }

        public async Task<EntityExistDto> TransactionExist(CreateOrEditTransactionDto input)
        {
            var entity = await _transactionRepository.FirstOrDefaultAsync(x => x.Pin == input.Pin && x.KeyType == input.KeyType && x.Transaction_Date.Date == input.Transaction_Date.Date);
            return new EntityExistDto { IsExist = entity != null ,Id = entity?.Id};
        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForProjectManager(GetTransactionDto input)
        {
            var project = _projectRepository.GetAllIncluding(x => x.Users).Where(x => x.Id == input.ProjectId && x.ManagerId == GetCurrentUser().Id).FirstOrDefault();
            var userIds = project.Users.Select(x => x.UserId).ToArray();

            var query = from  t1 in _transactionRepository.GetAll().Where(x => userIds.Contains(x.Pin))
                        join u1 in _lookup_userRepository.GetAll() on t1.Pin equals u1.Id
                        where t1.Transaction_Date.Date >= input.FromDate.Date && t1.Transaction_Date.Date <= input.ToDate.Date

                        select new GetTransactionForViewDto()
                        {
                            Transaction = new TransactionDto
                            {
                                TransType = t1.TransType,
                                KeyType = t1.KeyType,
                                CreationTime = t1.CreationTime,
                                Transaction_Date = t1.Transaction_Date,
                                Pin = t1.Pin,
                                Time = t1.Time,
                                Id = t1.Id,
                                ProjectManagerApprove = t1.ProjectManagerApprove,
                                UnitManagerApprove = t1.UnitManagerApprove
                            },
                            UserName = u1.Name == null ? "" : u1.Name.ToString(),
                            ProjectName = project.NameEn
                        };

            var data = await query.ToListAsync();

            var totalCount = await query.CountAsync();

            foreach (var item in data)
            {
                var userShifts = _UserShiftRepository.GetAll().Where(x => x.UserId == item.Transaction.Pin && x.Date.Date == item.Transaction.Transaction_Date.Date).ToList();

                if (userShifts.Count > 0)
                {
                    foreach (var userShift in userShifts)
                    {
                        
                        var minutes = (Double.Parse(item.Transaction.Time.Split(":")[0]) * 60) + (Double.Parse(item.Transaction.Time.Split(":")[1]));
                        //var TransTime = new DateTime(1990, 11, 20).AddMinutes(minutes);
                        var TransactionShift = _shiftRepository.FirstOrDefault(x => x.TimeInRangeFrom <= minutes && x.TimeOutRangeTo >= minutes && x.Id == userShift.ShiftId);
                        if (TransactionShift != null)
                        {
                            item.ShiftName = TransactionShift.NameEn;
                            item.TimeIn = TransactionShift.TimeIn;
                            item.TimeOut = TransactionShift.TimeOut;
                            item.LateIn = TransactionShift.LateIn;
                            item.LateOut = TransactionShift.LateOut;
                            item.EarlyIn = TransactionShift.EarlyIn;
                            item.EarlyOut = TransactionShift.EarlyOut;

                            

                            if (item.Transaction.KeyType == 1)
                            {
                                if ((int)minutes > TransactionShift.TimeIn)
                                    item.Attendance_LateIn = (int)minutes - TransactionShift.TimeIn;
                            }
                                

                            if (item.Transaction.KeyType == 2)
                            {
                                if ((int)minutes > TransactionShift.TimeOut)
                                {
                                    item.Overtime = (int)minutes - TransactionShift.TimeOut;
                                    data[0].TotalOverTime += (int)minutes - TransactionShift.TimeOut;
                                }
                                else
                                    item.Attendance_EarlyOut = TransactionShift.TimeOut - (int)minutes;
                            }
                                


                        }
                        

                    }
                }
                
            }
           

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
               data
            );
        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForUnitManager(GetTransactionDto input)
        {

             
            var project = _projectRepository.GetAllIncluding(x => x.Users).Where(x => x.Id == input.ProjectId).FirstOrDefault();
            var userIds = project.Users.Select(x => x.UserId).ToArray();

            var query = from t1 in _transactionRepository.GetAll().Where(x => userIds.Contains(x.Pin))
                        join u1 in _lookup_userRepository.GetAll() on t1.Pin equals u1.Id
                        where  t1.Transaction_Date.Date >= input.FromDate.Date && t1.Transaction_Date.Date <= input.ToDate.Date
                        select new GetTransactionForViewDto()
                        {
                            Transaction = new TransactionDto
                            {
                                TransType = t1.TransType,
                                KeyType = t1.KeyType,
                                CreationTime = t1.CreationTime,
                                Transaction_Date = t1.Transaction_Date,
                                Pin = t1.Pin,
                                Time = t1.Time,
                                Id = t1.Id,
                                ProjectManagerApprove = t1.ProjectManagerApprove,
                                UnitManagerApprove = t1.UnitManagerApprove
                            },
                            UserName = u1.Name == null ? "" : u1.Name.ToString(),
                            ProjectName = project.NameEn
                        };

            var data = await query.ToListAsync();

            var totalCount = await query.CountAsync();
           
            foreach (var item in data)
            {
                var userShifts = _UserShiftRepository.GetAll().Where(x => x.UserId == item.Transaction.Pin && x.Date.Date == item.Transaction.Transaction_Date.Date).ToList();


                if (userShifts.Count > 0)
                {
                    foreach (var userShift in userShifts)
                    {

                        var minutes = (Double.Parse(item.Transaction.Time.Split(":")[0]) * 60) + (Double.Parse(item.Transaction.Time.Split(":")[1]));
                        //var TransTime = new DateTime(1990, 11, 20).AddMinutes(minutes);
                        var TransactionShift = _shiftRepository.FirstOrDefault(x => x.TimeInRangeFrom <= minutes && x.TimeOutRangeTo >= minutes && x.Id == userShift.ShiftId);
                        if (TransactionShift != null)
                        {
                            item.ShiftName = TransactionShift.NameEn;
                            item.TimeIn = TransactionShift.TimeIn;
                            item.TimeOut = TransactionShift.TimeOut;
                            item.LateIn = TransactionShift.LateIn;
                            item.LateOut = TransactionShift.LateOut;
                            item.EarlyIn = TransactionShift.EarlyIn;
                            item.EarlyOut = TransactionShift.EarlyOut;

                            if (item.Transaction.KeyType == 1)
                            {
                                if ((int)minutes > TransactionShift.TimeIn)
                                    item.Attendance_LateIn = (int)minutes - TransactionShift.TimeIn;
                            }


                            if (item.Transaction.KeyType == 2)
                            {
                                if ((int)minutes > TransactionShift.TimeOut)
                                {
                                    item.Overtime = (int)minutes - TransactionShift.TimeOut;
                                    data[0].TotalOverTime += (int)minutes - TransactionShift.TimeOut;
                                }
                                else
                                    item.Attendance_EarlyOut = TransactionShift.TimeOut - (int)minutes;
                            }


                        }


                    }
                }
                
            }
            

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
               data
            );
        }

        public async Task BulkUpdateTransactions(List<GetTransactionForViewDto> input)
        {
            foreach (var transactionViewModel in input)
            {
                var transaction = await _transactionRepository.FirstOrDefaultAsync(transactionViewModel.Transaction.Id);
                ObjectMapper.Map(transactionViewModel.Transaction, transaction);
            }
        }

        public async Task UpdateSingleTransaction(GetTransactionForViewDto input)
        {
                var transaction = await _transactionRepository.FirstOrDefaultAsync(input.Transaction.Id);
                ObjectMapper.Map(input.Transaction, transaction);
        }

        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForHr(GetTransactionDto input)
        {
            var project = _projectRepository.GetAllIncluding(x => x.Users).Where(x => x.Id == input.ProjectId).FirstOrDefault();
            var userIds = project.Users.Select(x => x.UserId).ToArray();

            var query = from t1 in _transactionRepository.GetAll().Where(x => userIds.Contains(x.Pin))
                        join u1 in _lookup_userRepository.GetAll() on t1.Pin equals u1.Id
                        where t1.Transaction_Date.Date >= input.FromDate.Date && t1.Transaction_Date.Date <= input.ToDate.Date

                        select new GetTransactionForViewDto()
                        {
                            Transaction = new TransactionDto
                            {
                                TransType = t1.TransType,
                                KeyType = t1.KeyType,
                                CreationTime = t1.CreationTime,
                                Transaction_Date = t1.Transaction_Date,
                                Pin = t1.Pin,
                                Time = t1.Time,
                                Id = t1.Id,
                                ProjectManagerApprove = t1.ProjectManagerApprove,
                                UnitManagerApprove = t1.UnitManagerApprove
                            },
                            UserName = u1.Name == null ? "" : u1.Name.ToString(),
                            ProjectName = project.NameEn
                        };

            var data = await query.ToListAsync();

            var totalCount = await query.CountAsync();

            foreach (var item in data)
            {
                var userShifts = _UserShiftRepository.GetAll().Where(x => x.UserId == item.Transaction.Pin && x.Date.Date == item.Transaction.Transaction_Date.Date).ToList();

                if (userShifts.Count > 0)
                {
                    foreach (var userShift in userShifts)
                    {

                        var minutes = (Double.Parse(item.Transaction.Time.Split(":")[0]) * 60) + (Double.Parse(item.Transaction.Time.Split(":")[1]));
                        //var TransTime = new DateTime(1990, 11, 20).AddMinutes(minutes);
                        var TransactionShift = _shiftRepository.FirstOrDefault(x => x.TimeInRangeFrom <= minutes && x.TimeOutRangeTo >= minutes && x.Id == userShift.ShiftId);
                        if (TransactionShift != null)
                        {
                            item.ShiftName = TransactionShift.NameEn;
                            item.TimeIn = TransactionShift.TimeIn;
                            item.TimeOut = TransactionShift.TimeOut;
                            item.LateIn = TransactionShift.LateIn;
                            item.LateOut = TransactionShift.LateOut;
                            item.EarlyIn = TransactionShift.EarlyIn;
                            item.EarlyOut = TransactionShift.EarlyOut;

                            if (item.Transaction.KeyType == 1)
                            {
                                if ((int)minutes > TransactionShift.TimeIn)
                                    item.Attendance_LateIn = (int)minutes - TransactionShift.TimeIn;
                            }


                            if (item.Transaction.KeyType == 2)
                            {
                                if ((int)minutes > TransactionShift.TimeOut)
                                {
                                    item.Overtime = (int)minutes - TransactionShift.TimeOut;
                                    data[0].TotalOverTime += (int)minutes - TransactionShift.TimeOut;
                                }
                                    
                                else
                                    item.Attendance_EarlyOut = TransactionShift.TimeOut - (int)minutes;
                            }


                        }


                    }
                }
                
            }


            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
               data
            );
        }

        //project manager
        public async Task<ActualSummerizeTimeSheetOutput> GetActualSummerizeTimeSheet(ActualSummerizeInput input)
        {
            //get all project machines 
            var project = await _projectRepository.GetAllIncluding(x => x.Machines).FirstOrDefaultAsync(x => x.Id == input.ProjectId);
            var machines = project.Machines.Select(x => x.MachineId).ToList();

            // get all transactions for these machines 
            var transactions = _transactionRepository.GetAllIncluding(x => x.User)
                               .Where(x => machines.Contains(x.MachineId))
                               .Where(x => x.Transaction_Date.Month == input.Month && x.Transaction_Date.Year == input.Year).ToList();

            var users = transactions.GroupBy(x => x.User.Id).Select(x => x.First().User).ToList();
           


            var firstDayOfMonth = new DateTime(input.Year, input.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //generate the output
            var output = new ActualSummerizeTimeSheetOutput();
            

            // add users
            foreach (var user in users)
            {
                var summaryToAdd = new ActualSummerizeTimeSheetDto();
                summaryToAdd.UserId = user.Id;
                summaryToAdd.UserName = user.Name;
                summaryToAdd.Code = user.Code;
               
                output.UserIds.Add(new UserTimeSheetInput() { UserId = user.Id });

                // add days to users 
                for (var day = firstDayOfMonth.Date; day <= lastDayOfMonth.Date; day = day.AddDays(1))
                {
                    var detailToAdd = new ActualSummerizeTimeSheetDetailDto();
                    detailToAdd.Day = day;

                    var userTransactions = transactions.Where(x => x.Pin == user.Id && x.Transaction_Date.Date == day.Date).Select(x => x.Time).ToList();
                    var transCount = userTransactions.Count();

                    if (transCount == 0)
                    {
                        //check for vacation
                        var hasVacation = _employeeVacation.FirstOrDefault(x => x.UserId == user.Id && day >= x.FromDate && day <= x.ToDate);
                        if (hasVacation != null)
                            detailToAdd.IsLeave = true;
                        else
                            detailToAdd.IsAbsent = true;

                    }
                    else if (transCount == 2)
                    {
                        double inMinutes = 0;
                        double outMinutes = 0;
                        //in transaction
                        var inTransaction = userTransactions.FirstOrDefault();
                        if (!string.IsNullOrEmpty(inTransaction))
                         inMinutes = (Double.Parse(inTransaction.Split(":")[0]) * 60) + (Double.Parse(inTransaction.Split(":")[1]));

                        var outTransaction = userTransactions.Skip(transCount - 1).FirstOrDefault();
                        if (!string.IsNullOrEmpty(outTransaction))
                            outMinutes = (Double.Parse(outTransaction.Split(":")[0]) * 60) + (Double.Parse(outTransaction.Split(":")[1]));

                        detailToAdd.TotalHours = outMinutes - inMinutes;
                        if (detailToAdd.TotalHours > 480) // 8 hours
                        {
                            detailToAdd.Overtime = detailToAdd.TotalHours - 480; // 8 hours
                            if (!user.IsFixedOverTimeAllowed)
                            {
                                // 4 hours 
                                if (detailToAdd.Overtime > 240) {
                                    var timeToDeduct = detailToAdd.Overtime - 240;
                                    detailToAdd.Overtime = detailToAdd.Overtime - timeToDeduct;
                                }  
                            }
                        }
                        else if(detailToAdd.TotalHours < 480)
                        {
                            detailToAdd.IsDelay = true;
                            detailToAdd.Delay = 480 - detailToAdd.TotalHours;
                        }

                    } 
                    summaryToAdd.Details.Add(detailToAdd);
                }
                    

                output.Data.Add(summaryToAdd);
            }

            return output;


        }

        //unit manager
        public async Task<ActualSummerizeTimeSheetOutput> GetMangerUsersToApprove(ActualSummerizeInput input)
        {
            var output = new ActualSummerizeTimeSheetOutput();
            //get all project machines 
            var project = await _projectRepository.GetAllIncluding(x => x.Machines).FirstOrDefaultAsync(x => x.Id == input.ProjectId);
            var machines = project.Machines.Select(x => x.MachineId).ToList();

            // get all transactions for these machines 
            var transactions = _transactionRepository.GetAllIncluding(x => x.User)
                               .Where(x => machines.Contains(x.MachineId))
                               .Where(x => x.Transaction_Date.Month == input.Month && x.Transaction_Date.Year == input.Year).ToList();


         
            
            var allUnits = _organizationUnitRepository.GetAll().ToList();
            var managerUnit = await _organizationUnitRepository.GetAllIncluding(x => x.Children).Where(x => x.ManagerId == GetCurrentUser().Id).FirstOrDefaultAsync();
            
            var units = new List<long?>();
            var unitsToApprove = new List<long?>();
            units.Add(managerUnit.Id);
            var childs = GetChildes(unitsToApprove, managerUnit, allUnits);
            units.AddRange(childs);
            
            output.RemainingUnitsApprove = allUnits.Where(x => output.ParentUnitIds.Contains(x.Id)).Select(x => x.DisplayName).ToList();


            //
            var users = transactions.GroupBy(x => x.User.Id).Select(x => x.First().User);
            var managerUsers = users.Where(x => units.Contains(x.OrganizationUnitId)).ToList();
            

            var firstDayOfMonth = new DateTime(input.Year, input.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //generate the output
            

            // add users
            foreach (var user in managerUsers)
            {
                var summaryToAdd = new ActualSummerizeTimeSheetDto();
                summaryToAdd.UserId = user.Id;
                summaryToAdd.UserName = user.Name;
                summaryToAdd.Code = user.Code;
                var userTimeSheet = _userTimeSheetApproveRepository.FirstOrDefault(x => x.UserId == user.Id && x.ProjectManagerApprove == true && x.Month == input.Month && x.Year == input.Year && x.ProjectId == input.ProjectId);
                // check if project manager approve 
                if(userTimeSheet  != null)
                {
                    summaryToAdd.IsProjectManagerApproved = true;

                    // check if there is pending approve 
                    var userUnit = await _organizationUnitRepository.GetAll().Where(x => x.Id == user.OrganizationUnitId).FirstOrDefaultAsync();
                    var userUnitParents = new List<long>();
                    var userUnitParentsToApprove = new List<long>();
                    var parents =   GetParents(userUnitParentsToApprove, userUnit, allUnits);
                    userUnitParents.AddRange(parents);
                    userUnitParents.Add(userUnit.Id);

                    var currentunit = GetCurrentUser().OrganizationUnitId;

                    var remainingUnits = new List<long>();
                    var approvedUnits = new List<long>();
                    if (!string.IsNullOrEmpty(userTimeSheet.ApprovedUnits))
                    {
                        approvedUnits = userTimeSheet.ApprovedUnits.Split(",").Select(long.Parse).ToList();
                        summaryToAdd.IsCurrentUnitApproved = approvedUnits.Any(x => x == currentunit);
                    }

                    remainingUnits = userUnitParents.Where(x => !approvedUnits.Contains(x)).Select(x => x).ToList();
                    if (approvedUnits.Any(x => x == userUnit.Id))
                    {
                        
                        //check if current unit is the next unit to approve 
                        var nextUnitToApprove = remainingUnits.Where(x => x != userUnit.Id).OrderByDescending(x => x).FirstOrDefault();
                     

                        if (nextUnitToApprove == currentunit)
                        {
                            summaryToAdd.CanManagerApprove = true;
                            
                            //last one
                            if (remainingUnits.Count == 1)
                                summaryToAdd.YesClose = true;
                            
                            
                            output.UserIds.Add(new UserTimeSheetInput() { UserId = user.Id, YesClose = summaryToAdd.YesClose });
                        }

                    }
                    else
                    {
                        var nextUnitToApprove = remainingUnits.OrderByDescending(x => x).FirstOrDefault();
                       
                        if (nextUnitToApprove == currentunit)
                        {
                            summaryToAdd.CanManagerApprove = true;
                            //last one
                            if (remainingUnits.Count == 1)
                                summaryToAdd.YesClose = true;

                            output.UserIds.Add(new UserTimeSheetInput() { UserId = user.Id, YesClose = summaryToAdd.YesClose });
                        }
                    }
                }
                // add days to users 
                for (var day = firstDayOfMonth.Date; day <= lastDayOfMonth.Date; day = day.AddDays(1))
                {
                    var detailToAdd = new ActualSummerizeTimeSheetDetailDto();
                    detailToAdd.Day = day;

                    var userTransactions = transactions.Where(x => x.Pin == user.Id && x.Transaction_Date.Date == day.Date).Select(x => x.Time).ToList();
                    var transCount = userTransactions.Count();

                    if (transCount == 0)
                    {
                        //check for vacation
                        var hasVacation = _employeeVacation.FirstOrDefault(x => x.UserId == user.Id && day >= x.FromDate && day <= x.ToDate);
                        if (hasVacation != null)
                            detailToAdd.IsLeave = true;
                        else
                            detailToAdd.IsAbsent = true;

                    }
                    else if (transCount == 2)
                    {
                        double inMinutes = 0;
                        double outMinutes = 0;
                        //in transaction
                        var inTransaction = userTransactions.FirstOrDefault();
                        if (!string.IsNullOrEmpty(inTransaction))
                            inMinutes = (Double.Parse(inTransaction.Split(":")[0]) * 60) + (Double.Parse(inTransaction.Split(":")[1]));

                        var outTransaction = userTransactions.Skip(transCount - 1).FirstOrDefault();
                        if (!string.IsNullOrEmpty(outTransaction))
                            outMinutes = (Double.Parse(outTransaction.Split(":")[0]) * 60) + (Double.Parse(outTransaction.Split(":")[1]));

                        detailToAdd.TotalHours = outMinutes - inMinutes;
                        if (detailToAdd.TotalHours > 480) // 8 hours
                        {
                            detailToAdd.Overtime = detailToAdd.TotalHours - 480; // 8 hours
                            if (!user.IsFixedOverTimeAllowed)
                            {
                                // 4 hours 
                                if (detailToAdd.Overtime > 240)
                                {
                                    var timeToDeduct = detailToAdd.Overtime - 240;
                                    detailToAdd.Overtime = detailToAdd.Overtime - timeToDeduct;
                                }
                            }
                        }
                        else if (detailToAdd.TotalHours < 480)
                        {
                            detailToAdd.IsDelay = true;
                            detailToAdd.Delay = 480 - detailToAdd.TotalHours;
                        }

                    }
                    summaryToAdd.Details.Add(detailToAdd);
                }


                output.Data.Add(summaryToAdd);
               
            }
            output.CanApprove = !output.Data.Any(x => x.CanManagerApprove == false);
            return output;
        }
        public async Task PojectManagerApprove(ProjectManagerApproveInput input)
        {
            if (input.UserIds.Count > 0)
            {
                foreach (var user in input.UserIds)
                {
                    var userTimeSheet = _userTimeSheetApproveRepository.FirstOrDefault(x => x.UserId == user.UserId && x.Month == input.Month && x.Year == input.Year && x.ProjectId == input.ProjectId);
                    if (userTimeSheet == null)
                    {
                        var userTimeSheetToAdd = new UserTimeSheetApprove();
                        userTimeSheetToAdd.UserId = user.UserId;
                        userTimeSheetToAdd.ProjectId = input.ProjectId;
                        userTimeSheetToAdd.ProjectManagerApprove = true;
                        userTimeSheetToAdd.Year = input.Year;
                        userTimeSheetToAdd.Month = input.Month;
                        userTimeSheetToAdd.ProjectManagerId = GetCurrentUser().Id;

                        await _userTimeSheetApproveRepository.InsertAsync(userTimeSheetToAdd);
                    }
                    else
                    {
                        userTimeSheet.ProjectManagerApprove = true;
                        await _userTimeSheetApproveRepository.UpdateAsync(userTimeSheet);

                    }
                }
            }
            
        }
        public async Task PojectManagerReject(ProjectManagerApproveInput input)
        {
            if (input.UserIds.Count > 0)
            {
                foreach (var user in input.UserIds)
                {
                    //check if exist 
                    var userTimeSheet = _userTimeSheetApproveRepository.FirstOrDefault(x => x.UserId == user.UserId && x.Month == input.Month && x.Year == input.Year && x.ProjectId == input.ProjectId);
                    if(userTimeSheet != null)
                    {
                        userTimeSheet.ProjectManagerApprove = false;
                        await _userTimeSheetApproveRepository.UpdateAsync(userTimeSheet);
                    }
                    else
                    {
                        var userTimeSheetToAdd = new UserTimeSheetApprove();
                        userTimeSheetToAdd.UserId = user.UserId;
                        userTimeSheetToAdd.ProjectId = input.ProjectId;
                        userTimeSheetToAdd.ProjectManagerApprove = false;
                        userTimeSheetToAdd.Year = input.Year;
                        userTimeSheetToAdd.Month = input.Month;
                        userTimeSheetToAdd.ProjectManagerId = GetCurrentUser().Id;

                        await _userTimeSheetApproveRepository.InsertAsync(userTimeSheetToAdd);
                    }
                  
                }
            }
        }

        public async Task UnitManagerToApprove(ProjectManagerApproveInput input)
        {
            var loggedInManagerUnit = GetCurrentUser().OrganizationUnitId;

            foreach (var user in input.UserIds)
            {
                var approvedUnits = new List<long>();
                var uerToUpdate = await _userTimeSheetApproveRepository.FirstOrDefaultAsync(x => x.UserId == user.UserId);
                if(uerToUpdate.ApprovedUnits != null)
                    approvedUnits =  uerToUpdate.ApprovedUnits.Split(',').Select(long.Parse).ToList();

                if (!approvedUnits.Any(x => x == loggedInManagerUnit.Value))
                {
                    approvedUnits.Add(loggedInManagerUnit.Value);
                }

                uerToUpdate.ApprovedUnits = string.Join(",", approvedUnits);
                uerToUpdate.IsClosed = user.YesClose;
                await _userTimeSheetApproveRepository.UpdateAsync(uerToUpdate);
            }
           
        }
        private static List<long> GetParents(List <long> parents , OrganizationUnitExtended unit , List<OrganizationUnitExtended> units)
        {
            if (unit.ParentId != null )
            {
                var parent = units.FirstOrDefault(d => d.Id == unit.ParentId);
                if (parent != null || parent.HasApprove)
                    parents.Add(parent.Id);

                if (parent.ParentId != null && unit.HasApprove)
                    GetParents(parents,parent, units);
            }

            return parents;



        }

        private List<long?> GetChildes(List<long?> childs, OrganizationUnitExtended unit, List<OrganizationUnitExtended> units)
        {
            if (unit.Children.Count > 0)
            {
                foreach (var child in unit.Children)
                {
                    childs.Add(child.Id);
                    var newEntity = _organizationUnitRepository.GetAllIncluding(x => x.Children).FirstOrDefault(d => d.Id == child.Id);
                    if (newEntity.Children.Count > 0)
                    {
                        GetChildes(childs, newEntity, units);
                    }
                }

            }

            return childs;



        }
    }
}