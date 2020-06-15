using System.Collections.Generic;
using Pixel.Attendance.Authorization.Users.Importing.Dto;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
