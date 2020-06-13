
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.GEC.Attendance.Attendance.Exporting;
using Pixel.GEC.Attendance.Attendance.Dtos;
using Pixel.GEC.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Authorization;
using Pixel.GEC.Attendance.Authorization.Users;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.Setting;
using Microsoft.AspNetCore.Identity;
using Pixel.GEC.Attendance.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Pixel.GEC.Attendance.Attendance
{
    [AbpAuthorize]
    [Authorize()]
    //[AbpAuthorize(AppPermissions.Pages_MobileTransactions)]
    public class MobileTransactionsAppService : AttendanceAppServiceBase, IMobileTransactionsAppService
    {
        private readonly IRepository<MobileTransaction> _mobileTransactionRepository;
        private readonly IMobileTransactionsExcelExporter _mobileTransactionsExcelExporter;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<LocationCredential> _locationCredentialRepository;
        private readonly UserManager _userManager;


        public MobileTransactionsAppService(IRepository<MobileTransaction> mobileTransactionRepository,
            IMobileTransactionsExcelExporter mobileTransactionsExcelExporter,
            IRepository<LocationCredential> locationCredentialRepository,
            IRepository<Location> locationRepository,
        UserManager userManager)

        {
            _mobileTransactionRepository = mobileTransactionRepository;
            _mobileTransactionsExcelExporter = mobileTransactionsExcelExporter;
            _locationCredentialRepository = locationCredentialRepository;
            _locationRepository = locationRepository;
            _userManager = userManager;
        }

        public async Task<PagedResultDto<GetMobileTransactionForViewDto>> GetAll(GetAllMobileTransactionsInput input)
        {

            var filteredMobileTransactions = _mobileTransactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmpCode.Contains(input.Filter) || e.MachineID.Contains(input.Filter) || e.CivilId.Contains(input.Filter) || e.Image.Contains(input.Filter) || e.SiteName.Contains(input.Filter))
                        .WhereIf(input.MinSiteIdFilter != null, e => e.SiteId >= input.MinSiteIdFilter)
                        .WhereIf(input.MaxSiteIdFilter != null, e => e.SiteId <= input.MaxSiteIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SiteNameFilter), e => e.SiteName == input.SiteNameFilter);

            var pagedAndFilteredMobileTransactions = filteredMobileTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mobileTransactions = from o in pagedAndFilteredMobileTransactions
                                     select new GetMobileTransactionForViewDto()
                                     {
                                         MobileTransaction = new MobileTransactionDto
                                         {
                                             SiteId = o.SiteId,
                                             SiteName = o.SiteName,
                                             Id = o.Id
                                         }
                                     };

            var totalCount = await filteredMobileTransactions.CountAsync();

            return new PagedResultDto<GetMobileTransactionForViewDto>(
                totalCount,
                await mobileTransactions.ToListAsync()
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_MobileTransactions_Edit)]
        public async Task<GetMobileTransactionForEditOutput> GetMobileTransactionForEdit(EntityDto input)
        {
            var mobileTransaction = await _mobileTransactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMobileTransactionForEditOutput { MobileTransaction = ObjectMapper.Map<CreateOrEditMobileTransactionDto>(mobileTransaction) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMobileTransactionDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_MobileTransactions_Create)]
        protected virtual async Task Create(CreateOrEditMobileTransactionDto input)
        {
            var mobileTransaction = ObjectMapper.Map<MobileTransaction>(input);



            await _mobileTransactionRepository.InsertAsync(mobileTransaction);
        }

        //[AbpAuthorize(AppPermissions.Pages_MobileTransactions_Edit)]
        protected virtual async Task Update(CreateOrEditMobileTransactionDto input)
        {
            var mobileTransaction = await _mobileTransactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mobileTransaction);
        }

        //[AbpAuthorize(AppPermissions.Pages_MobileTransactions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mobileTransactionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMobileTransactionsToExcel(GetAllMobileTransactionsForExcelInput input)
        {

            var filteredMobileTransactions = _mobileTransactionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmpCode.Contains(input.Filter) || e.MachineID.Contains(input.Filter) || e.CivilId.Contains(input.Filter) || e.Image.Contains(input.Filter) || e.SiteName.Contains(input.Filter))
                        .WhereIf(input.MinSiteIdFilter != null, e => e.SiteId >= input.MinSiteIdFilter)
                        .WhereIf(input.MaxSiteIdFilter != null, e => e.SiteId <= input.MaxSiteIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SiteNameFilter), e => e.SiteName == input.SiteNameFilter);

            var query = (from o in filteredMobileTransactions
                         select new GetMobileTransactionForViewDto()
                         {
                             MobileTransaction = new MobileTransactionDto
                             {
                                 SiteId = o.SiteId,
                                 SiteName = o.SiteName,
                                 Id = o.Id
                             }
                         });


            var mobileTransactionListDtos = await query.ToListAsync();

            return _mobileTransactionsExcelExporter.ExportToFile(mobileTransactionListDtos);
        }

        [HttpPost]
        public async Task<CheckEmpLocationOutputModel> CheckEmpLocation([FromBody] CheckEmpLocationInputModel input)
        {

            var user = await _userManager.Users.Include(x => x.Locations).Where(x => x.CivilId == input.CivilID).FirstOrDefaultAsync();
            var locations = user.Locations.Where(x => x.FromDate.Date <= DateTime.Today & x.ToDate.Date >= DateTime.Today);
            foreach (var item in locations)
            {
                var location = _locationRepository.GetAll().Where(x => x.Id == item.LocationId).FirstOrDefault();
                List<Point> points = new List<Point>();
                var crds = _locationCredentialRepository.GetAll().Where(x => x.LocationId == item.LocationId);
                foreach (var crd in crds)
                {
                    points.Add(new Point(crd.Latitude, crd.Longitude));
                }
                Point currentpoint = new Point(input.Latitude, input.Longitude);
                Polygon polygon = new Polygon(points);
                if (polygon.IsPointInPolygon(currentpoint))
                {
                    return new CheckEmpLocationOutputModel
                    {
                        Status = true,
                        SiteID = item.LocationId,
                        SiteName = (input.LanguaugeId == 1) ? location.TitleAr : location.TitleEn,
                        FromDate = item.FromDate,
                        EndDate = item.ToDate
                    };
                }

            }
            MobileTransaction transaction = new MobileTransaction
            {
                EmpCode = user.Code,
                CivilId = input.CivilID,
                TransType = "Fail",
                Latitude = input.Latitude,
                Longitude = input.Longitude,
                TransStatus = false,
                TransDate = DateTime.Now
            };
            _mobileTransactionRepository.Insert(transaction);
            return new CheckEmpLocationOutputModel
            {
                Status = false,
                Message = (input.LanguaugeId == 1) ? "غير مسموح بالبصمة فى هذا المكان" : "Not allowed"
            };
        }
        [HttpPost]
        public async Task<InsertTransactionOutputModel> InsertTransaction([FromBody] InsertTransactionInputModel input)
        {
            if (!CheckEmpLocation(new CheckEmpLocationInputModel { CivilID = input.CivilId, Latitude = input.Lat, Longitude = input.Long }).Result.Status)
            {
                return new InsertTransactionOutputModel
                {
                    Status = false,
                    Message = (input.LanguaugeId == 1) ? "غير مسموح بالبصمه فى هذا الموقع" : "Transaction Not Allowed"
                };
            }
            MobileTransaction mobileTransaction = new MobileTransaction
            {
                CivilId = input.CivilId,
                Image = input.ZoomAuditImage,
                Longitude = input.Long,
                Latitude = input.Lat,
                TransType = input.Type,
                SiteId = input.SiteId,
                SiteName = input.SiteName,
                EmpCode = input.EmpCode,
                TransDate = DateTime.Now,
                TransStatus = true,
                MachineID = "Mobile"
            };
            try
            {
                await _mobileTransactionRepository.InsertAsync(mobileTransaction);
                return new InsertTransactionOutputModel
                {
                    Status = true,
                    date = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToString("hh:mm:ss"),
                    Message = (input.Type == "IN") ? ((input.LanguaugeId == 1) ? "تم اثبات بصمة الحضور" : "Checked in Succesfully") : ((input.LanguaugeId == 1) ? "تم اثبات بصمة الانصراف" : "Checked Out Succesfully")
                };
            }
            catch (Exception ex)
            {
                return new InsertTransactionOutputModel
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
        [HttpPost]
        public async Task<ReportOutputModel> Report([FromBody] ReportInputModel input)
        {
            try
            {
                var transactions = await _mobileTransactionRepository.GetAll().Where(x => x.CivilId == input.CivilId
                                                                            & x.TransDate.Date >= input.FromDate.Date
                                                                            & x.TransDate.Date <= input.ToDate.Date).OrderByDescending(x => x.TransDate.Date).ToListAsync();
                if (transactions.Count == 0)
                {
                    return new ReportOutputModel
                    {
                        Status = false,
                        Message = (input.LanguaugeId == 1) ? "لا يوجد بصمات" : "No Transactions Found"
                    };
                }
                List<TransEntityModel> model = new List<TransEntityModel>();
                var result = transactions;
                DateTime tempdate = DateTime.Now.AddDays(1);
                foreach (var item in result)
                {
                    if (item.TransDate.Date != tempdate.Date)
                    {
                        var lst = transactions.Where(x => x.TransDate.Date == item.TransDate.Date).ToList();
                        TransEntityModel report = new TransEntityModel
                        {
                            TransDate = item.TransDate.Date,
                            Transactions = ObjectMapper.Map<List<ReportEntityModel>>(lst)
                        };
                        model.Add(report);
                    }
                    tempdate = item.TransDate.Date;
                }

                return new ReportOutputModel
                {
                    Status = true,
                    Transactions = model
                };

            }
            catch (Exception ex)
            {

                return new ReportOutputModel
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
        [HttpPost]
        public async Task<LastTransOutputModel> GetLastTransaction([FromBody] LastTransactionInputModel input)
        {
            try
            {
                var transactions = await _mobileTransactionRepository.GetAll().Where(x => x.CivilId == input.CivilId & x.TransDate.Date == DateTime.Today).OrderByDescending(x => x.TransDate).ToListAsync();
                if (transactions.Count == 0)
                {
                    return new LastTransOutputModel
                    {
                        Status = false,
                        Message = (input.LanguaugeId == 1) ? "لا يوجد بصمات" : "No Transactions Found"
                    };
                }
                return new LastTransOutputModel
                {
                    Status = true,
                    Transactions = ObjectMapper.Map<List<ReportEntityModel>>(transactions)
                };
            }
            catch (Exception ex)
            {

                return new LastTransOutputModel
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
        [HttpPost]
        public async Task<GetEmpLocationsOutputModel> GetEmpLocations([FromBody] ReportInputModel input)
        {
            try
            {
                List<LocationModel> model = new List<LocationModel>();
                var user = await _userManager.Users.Include(x => x.Locations).Where(x => x.CivilId == input.CivilId).FirstOrDefaultAsync();
                var locations = user.Locations.Where(x => x.ToDate.Date >= DateTime.Now.Date);

                foreach (var item in locations)
                {
                    var location = _locationRepository.GetAll().Where(x => x.Id == item.LocationId).FirstOrDefault();
                    LocationModel locationmodel = new LocationModel { Siteid = item.LocationId, SiteName = location.TitleAr, FromDate = item.FromDate, ToDate = item.FromDate };
                    var crds = _locationCredentialRepository.GetAll().Where(x => x.LocationId == item.LocationId);
                    List<LocationCerdentialModel> cerdentiallist = new List<LocationCerdentialModel>();
                    foreach (var crd in crds)
                    {
                        cerdentiallist.Add(new LocationCerdentialModel { lat = crd.Latitude, lon = crd.Longitude });
                    }
                    locationmodel.cerdentials = cerdentiallist;
                    model.Add(locationmodel);
                }
                if (locations.Count() > 0)
                {
                    return new GetEmpLocationsOutputModel { Status = true, locations = model };
                }
                else
                {
                    return new GetEmpLocationsOutputModel { Status = false, Message = (input.LanguaugeId == 1) ? "لا يوجد مواقع متاحة للبصمة" : "No Locations Avaliable" };
                }

            }
            catch (Exception ex)
            {
                return new GetEmpLocationsOutputModel { Status = false, Message = ex.Message };
            }
        }

       
    }
}
