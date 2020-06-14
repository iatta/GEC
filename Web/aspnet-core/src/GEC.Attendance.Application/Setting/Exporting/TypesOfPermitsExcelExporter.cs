using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Setting.Exporting
{
    public class TypesOfPermitsExcelExporter : EpPlusExcelExporterBase, ITypesOfPermitsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TypesOfPermitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTypesOfPermitForViewDto> typesOfPermits)
        {
            return CreateExcelPackage(
                "TypesOfPermits.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TypesOfPermits"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NameAr"),
                        L("NameEn")
                        );

                    AddObjects(
                        sheet, 2, typesOfPermits,
                        _ => _.TypesOfPermit.NameAr,
                        _ => _.TypesOfPermit.NameEn
                        );

					

                });
        }
    }
}
