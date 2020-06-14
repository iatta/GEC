using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Operations.Exporting
{
    public class EmployeeOfficialTasksExcelExporter : EpPlusExcelExporterBase, IEmployeeOfficialTasksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeOfficialTasksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeOfficialTaskForViewDto> employeeOfficialTasks)
        {
            return CreateExcelPackage(
                "EmployeeOfficialTasks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeOfficialTasks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        L("Remarks"),
                        L("DescriptionAr"),
                        L("DescriptionEn"),
                        (L("OfficialTaskType")) + L("NameAr")
                        );

                    AddObjects(
                        sheet, 2, employeeOfficialTasks,
                        _ => _timeZoneConverter.Convert(_.EmployeeOfficialTask.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.EmployeeOfficialTask.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeOfficialTask.Remarks,
                        _ => _.EmployeeOfficialTask.DescriptionAr,
                        _ => _.EmployeeOfficialTask.DescriptionEn,
                        _ => _.OfficialTaskTypeNameAr
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
