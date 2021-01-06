using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Setting.Exporting
{
    public class RamadanDatesExcelExporter : EpPlusExcelExporterBase, IRamadanDatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RamadanDatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRamadanDateForViewDto> ramadanDates)
        {
            return CreateExcelPackage(
                "RamadanDates.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("RamadanDates"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Year"),
                        L("FromDate"),
                        L("ToDate")
                        );

                    AddObjects(
                        sheet, 2, ramadanDates,
                        _ => _.RamadanDate.Year,
                        _ => _timeZoneConverter.Convert(_.RamadanDate.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.RamadanDate.ToDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var fromDateColumn = sheet.Column(2);
                    fromDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					fromDateColumn.AutoFit();
					var toDateColumn = sheet.Column(3);
                    toDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					toDateColumn.AutoFit();
					

                });
        }
    }
}
