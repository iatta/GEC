using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Setting;
using Pixel.Attendance.Web.Models;
using Rotativa.AspNetCore;

namespace Pixel.Attendance.Web.Controllers
{
    public class ReportController : AttendanceControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IWarningTypesAppService _warningTypesAppService;
        public ReportController( IUserAppService userAppService, IWarningTypesAppService warningsTypesAppService)
        {
            _userAppService = userAppService;
            _warningTypesAppService = warningsTypesAppService;
        }

        [HttpPost]
        public async Task<FileResult> GenerateInOutReport([FromBody] ReportInput input)
        {
            var data =await _userAppService.GenerateInOutReport(input);
            var query = from s in data
                        group s by s.EmpId into g
                        select g.ToList();

            var result = query.ToList();


            string cusomtSwitches = string.Format("--header-html \"{0}\" --footer-center \"  Created Date: " +
                                 DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                                 " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"", Url.Action("Header","Report",new HeaderModel { ReportName = "تقرير الحضور والانصراف "},"https"));
             var report = new ViewAsPdf("InOutReport")
            {
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 30 },
                Model = result,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = cusomtSwitches
             };
                
           var bytes = await report.BuildFile(this.ControllerContext);
            return File(bytes, "application/pdf", "test.pdf");

        }

        [HttpPost]
        public async Task<FileResult> GenerateForgetInOutReport([FromBody] ReportInput input)
        {
            var data = await _userAppService.GenerateForgetInOutReport(input);
            var query = from s in data
                        group s by s.UnitId into g
                        select g.ToList();

            var result = query.ToList();


            string cusomtSwitches = string.Format("--header-html \"{0}\" --footer-center \"  Created Date: " +
                                 DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                                 " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"", Url.Action("Header", "Report", new HeaderModel { ReportName = "  تقرير نسيان بصمة دخول / خروج " }, "https"));
            var report = new ViewAsPdf("ForgetInOut")
            {
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 30 },
                Model = result,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = cusomtSwitches
            };

            var bytes = await report.BuildFile(this.ControllerContext);
            return File(bytes, "application/pdf", "test.pdf");

        }

        [HttpPost]
        public async Task<FileResult> GenerateFingerReport([FromBody] ReportInput input)
        {
            var data = await _userAppService.GenerateFingerReport(input);
            var query = from s in data
                        group s by s.UserId into g
                        select g.ToList();

            var result = query.ToList();


            string cusomtSwitches = string.Format("--header-html \"{0}\" --footer-center \"  Created Date: " +
                                 DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                                 " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"", Url.Action("Header", "Report", new HeaderModel { ReportName = "  تقرير البصمات " }, "https"));
            var report = new ViewAsPdf("FingerReport")
            {
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 30 },
                Model = result,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = cusomtSwitches
            };

            var bytes = await report.BuildFile(this.ControllerContext);
            return File(bytes, "application/pdf", "test.pdf");

        }
        [HttpPost]
        public async Task<FileResult> GenerateWarningeport([FromBody] CreateOrEditEmployeeWarningDto input)
        {
            var wariningModel = new EmployeeWarningReportPrint();
            wariningModel.WarningName =  _warningTypesAppService.GetWarningTypeForView(input.WarningTypeId.Value).Result.WarningType.NameAr;
            wariningModel.FromDate = input.FromDate;
            wariningModel.ToDate = input.ToDate;

            string cusomtSwitches = string.Format("--header-html \"{0}\" --footer-center \"  Created Date: " +
                                 DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                                 " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\"", Url.Action("Header", "Report", new HeaderModel { ReportName = "الهيئة العامة لشئون ذوي الإعاقة" }, "https"));
            var report = new ViewAsPdf("WarningReport")
            {
                PageMargins = { Left = 5, Bottom = 20, Right = 5, Top = 30 },
                Model = wariningModel,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = cusomtSwitches
            };

            var bytes = await report.BuildFile(this.ControllerContext);
            return File(bytes, "application/pdf", "test.pdf");

        }

        [AllowAnonymous]
        public ActionResult Header(HeaderModel model)
        {
            return View(model);
        }
    }
}