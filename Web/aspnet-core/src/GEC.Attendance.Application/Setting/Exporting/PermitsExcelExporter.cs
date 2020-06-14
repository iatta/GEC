using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Setting.Exporting
{
    public class PermitsExcelExporter : EpPlusExcelExporterBase, IPermitsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PermitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPermitForViewDto> permits)
        {
            return CreateExcelPackage(
                "Permits.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Permits"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DescriptionAr"),
                        L("DescriptionEn"),
                        L("ToleranceIn"),
                        L("ToleranceOut"),
                        L("MaxNumberPerDay"),
                        L("MaxNumberPerWeek"),
                        L("MaxNumberPerMonth"),
                        L("TotalHoursPerDay"),
                        L("TotalHoursPerWeek"),
                        L("TotalHoursPerMonth")
                        );

                    AddObjects(
                        sheet, 2, permits,
                        _ => _.Permit.DescriptionAr,
                        _ => _.Permit.DescriptionEn,
                        _ => _.Permit.ToleranceIn,
                        _ => _.Permit.ToleranceOut,
                        _ => _.Permit.MaxNumberPerDay,
                        _ => _.Permit.MaxNumberPerWeek,
                        _ => _.Permit.MaxNumberPerMonth,
                        _ => _.Permit.TotalHoursPerDay,
                        _ => _.Permit.TotalHoursPerWeek,
                        _ => _.Permit.TotalHoursPerMonth
                        );

					

                });
        }
    }
}
