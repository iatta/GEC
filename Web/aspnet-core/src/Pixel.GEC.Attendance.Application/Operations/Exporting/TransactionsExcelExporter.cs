﻿using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public class TransactionsExcelExporter : EpPlusExcelExporterBase, ITransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionForViewDto> transactions)
        {
            return CreateExcelPackage(
                "Transactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Transactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TransType")
                        );

                    AddObjects(
                        sheet, 2, transactions,
                        _ => _.Transaction.TransType
                        );

					

                });
        }
    }
}
