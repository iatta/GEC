using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Operations.Exporting
{
    public class EmployeeVacationsExcelExporter : EpPlusExcelExporterBase, IEmployeeVacationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeVacationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeVacationForViewDto> employeeVacations)
        {
            return CreateExcelPackage(
                "EmployeeVacations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeVacations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        L("Description"),
                        L("Status"),
                        L("Note"),
                        (L("User")) + L("Name"),
                        (L("LeaveType")) + L("NameAr")
                        );

                    AddObjects(
                        sheet, 2, employeeVacations,
                        _ => _timeZoneConverter.Convert(_.EmployeeVacation.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.EmployeeVacation.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeVacation.Description,
                        _ => _.EmployeeVacation.Status,
                        _ => _.EmployeeVacation.Note,
                        _ => _.UserName,
                        _ => _.LeaveTypeNameAr
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
