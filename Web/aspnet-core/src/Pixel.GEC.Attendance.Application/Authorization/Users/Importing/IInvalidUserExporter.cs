using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Users.Importing.Dto;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
