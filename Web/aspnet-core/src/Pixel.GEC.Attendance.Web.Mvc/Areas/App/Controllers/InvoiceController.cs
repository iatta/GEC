﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.MultiTenancy.Accounting;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Accounting;
using Pixel.GEC.Attendance.Web.Controllers;

namespace Pixel.GEC.Attendance.Web.Areas.App.Controllers
{
    [Area("App")]
    public class InvoiceController : AttendanceControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new InvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}