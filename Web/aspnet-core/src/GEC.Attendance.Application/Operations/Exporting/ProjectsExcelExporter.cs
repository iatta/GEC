using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Operations.Exporting
{
    public class ProjectsExcelExporter : EpPlusExcelExporterBase, IProjectsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectForViewDto> projects)
        {
            return CreateExcelPackage(
                "Projects.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Projects"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NameAr"),
                        L("NameEn"),
                        (L("User")) + L("Name"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("Location")) + L("TitleEn")
                        );

                    AddObjects(
                        sheet, 2, projects,
                        _ => _.Project.NameAr,
                        _ => _.Project.NameEn,
                        _ => _.UserName,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.LocationTitleEn
                        );

					

                });
        }
    }
}
