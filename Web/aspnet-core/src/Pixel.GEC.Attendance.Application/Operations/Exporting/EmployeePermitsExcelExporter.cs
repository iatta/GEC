using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public class EmployeePermitsExcelExporter : EpPlusExcelExporterBase, IEmployeePermitsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeePermitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeePermitForViewDto> employeePermits)
        {
            return CreateExcelPackage(
                "EmployeePermits.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeePermits"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromTime"),
                        L("PermitDate"),
                        L("Description"),
                        L("Status"),
                        (L("User")) + L("Name"),
                        (L("Permit")) + L("DescriptionAr")
                        );

                    AddObjects(
                        sheet, 2, employeePermits,
                        _ => _.EmployeePermit.FromTime,
                        _ => _timeZoneConverter.Convert(_.EmployeePermit.PermitDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeePermit.Description,
                        _ => _.EmployeePermit.Status,
                        _ => _.UserName,
                        _ => _.PermitDescriptionAr
                        );

					var permitDateColumn = sheet.Column(2);
                    permitDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					permitDateColumn.AutoFit();
					

                });
        }
    }
}
