using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Operations.Exporting
{
    public class EmployeeWarningsExcelExporter : EpPlusExcelExporterBase, IEmployeeWarningsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeWarningsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeWarningForViewDto> employeeWarnings)
        {
            return CreateExcelPackage(
                "EmployeeWarnings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeWarnings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        (L("User")) + L("Name"),
                        (L("WarningType")) + L("NameAr")
                        );

                    AddObjects(
                        sheet, 2, employeeWarnings,
                        _ => _timeZoneConverter.Convert(_.EmployeeWarning.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.EmployeeWarning.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.WarningTypeNameAr
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
