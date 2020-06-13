﻿using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Setting.Exporting
{
    public class OfficialTaskTypesExcelExporter : EpPlusExcelExporterBase, IOfficialTaskTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OfficialTaskTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOfficialTaskTypeForViewDto> officialTaskTypes)
        {
            return CreateExcelPackage(
                "OfficialTaskTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OfficialTaskTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NameAr"),
                        L("NameEn"),
                        L("TypeIn"),
                        L("TypeOut"),
                        L("TypeInOut")
                        );

                    AddObjects(
                        sheet, 2, officialTaskTypes,
                        _ => _.OfficialTaskType.NameAr,
                        _ => _.OfficialTaskType.NameEn,
                        _ => _.OfficialTaskType.TypeIn,
                        _ => _.OfficialTaskType.TypeOut,
                        _ => _.OfficialTaskType.TypeInOut
                        );

					

                });
        }
    }
}
