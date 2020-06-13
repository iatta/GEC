﻿using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Setting.Exporting
{
    public class JobTitleExcelExporter : EpPlusExcelExporterBase, IJobTitleExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobTitleExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobTitleForViewDto> jobTitles)
        {
            return CreateExcelPackage(
                "Tests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Tests"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, jobTitles,
                        _ => _.JobTitle.NameAr
                        );

					

                });
        }
    }
}
