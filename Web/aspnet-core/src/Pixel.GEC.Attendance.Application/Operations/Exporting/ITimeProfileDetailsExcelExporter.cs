using System.Collections.Generic;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public interface ITimeProfileDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetTimeProfileDetailForViewDto> timeProfileDetails);
    }
}