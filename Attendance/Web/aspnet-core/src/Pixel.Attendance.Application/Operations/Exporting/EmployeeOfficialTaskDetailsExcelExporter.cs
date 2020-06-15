using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class EmployeeOfficialTaskDetailsExcelExporter : EpPlusExcelExporterBase, IEmployeeOfficialTaskDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeOfficialTaskDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeOfficialTaskDetailForViewDto> employeeOfficialTaskDetails)
        {
            return CreateExcelPackage(
                "EmployeeOfficialTaskDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeOfficialTaskDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("EmployeeOfficialTask")) + L("DescriptionAr"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, employeeOfficialTaskDetails,
                        _ => _.EmployeeOfficialTaskDescriptionAr,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
