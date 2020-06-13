﻿using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public class ManualTransactionsExcelExporter : EpPlusExcelExporterBase, IManualTransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ManualTransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetManualTransactionForViewDto> manualTransactions)
        {
            return CreateExcelPackage(
                "ManualTransactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ManualTransactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TransDate"),
                        L("TransType"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, manualTransactions,
                        _ => _timeZoneConverter.Convert(_.ManualTransaction.TransDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ManualTransaction.TransType,
                        _ => _.UserName
                        );

					var transDateColumn = sheet.Column(1);
                    transDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					transDateColumn.AutoFit();
					

                });
        }
    }
}
