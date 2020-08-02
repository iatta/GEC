using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class UserDelegationsExcelExporter : EpPlusExcelExporterBase, IUserDelegationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserDelegationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserDelegationForViewDto> userDelegations)
        {
            return CreateExcelPackage(
                "UserDelegations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserDelegations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FromDate"),
                        L("ToDate"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, userDelegations,
                        _ => _timeZoneConverter.Convert(_.UserDelegation.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.UserDelegation.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.UserName2
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
