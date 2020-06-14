using System.Collections.Generic;
using GEC.Attendance.Authorization.Users.Importing.Dto;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
