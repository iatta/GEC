using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class UserTimeSheetApprovesExcelExporter : EpPlusExcelExporterBase, IUserTimeSheetApprovesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserTimeSheetApprovesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserTimeSheetApproveForViewDto> userTimeSheetApproves)
        {
            return CreateExcelPackage(
                "UserTimeSheetApproves.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserTimeSheetApproves"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Month"),
                        L("Year"),
                        L("FromDate"),
                        L("ToDate"),
                        L("ApprovedUnits"),
                        L("ProjectManagerApprove"),
                        L("IsClosed"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("Project")) + L("NameEn")
                        );

                    AddObjects(
                        sheet, 2, userTimeSheetApproves,
                        _ => _.UserTimeSheetApprove.Month,
                        _ => _.UserTimeSheetApprove.Year,
                        _ => _timeZoneConverter.Convert(_.UserTimeSheetApprove.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.UserTimeSheetApprove.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserTimeSheetApprove.ApprovedUnits,
                        _ => _.UserTimeSheetApprove.ProjectManagerApprove,
                        _ => _.UserTimeSheetApprove.IsClosed,
                        _ => _.UserName,
                        _ => _.UserName2,
                        _ => _.ProjectNameEn
                        );

					var fromDateColumn = sheet.Column(3);
                    fromDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					fromDateColumn.AutoFit();
					var toDateColumn = sheet.Column(4);
                    toDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					toDateColumn.AutoFit();
					

                });
        }
    }
}
