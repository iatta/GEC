﻿using System.Collections.Generic;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Exporting
{
    public interface ILocationMachinesExcelExporter
    {
        FileDto ExportToFile(List<GetLocationMachineForViewDto> locationMachines);
    }
}