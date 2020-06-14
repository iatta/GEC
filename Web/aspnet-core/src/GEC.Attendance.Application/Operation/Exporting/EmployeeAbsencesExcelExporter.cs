using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Operation.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Operation.Exporting
{
    public class EmployeeAbsencesExcelExporter : EpPlusExcelExporterBase, IEmployeeAbsencesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeAbsencesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeAbsenceForViewDto> employeeAbsences)
        {
            return CreateExcelPackage(
                "EmployeeAbsences.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeAbsences"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, employeeAbsences,
                        _ => _timeZoneConverter.Convert(_.EmployeeAbsence.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.EmployeeAbsence.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
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
