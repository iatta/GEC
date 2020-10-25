using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class EmployeeTempTransfersExcelExporter : EpPlusExcelExporterBase, IEmployeeTempTransfersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeTempTransfersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeTempTransferForViewDto> employeeTempTransfers)
        {
            return CreateExcelPackage(
                "EmployeeTempTransfers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeTempTransfers"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, employeeTempTransfers,
                        _ => _timeZoneConverter.Convert(_.EmployeeTempTransfer.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.EmployeeTempTransfer.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.UserName
                        );

					var fromDateColumn = sheet.Column(1);
                    fromDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					fromDateColumn.AutoFit();
					var toDateColumn = sheet.Column(2);
                    toDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					toDateColumn.AutoFit();
					

                });
        }
    }
}
