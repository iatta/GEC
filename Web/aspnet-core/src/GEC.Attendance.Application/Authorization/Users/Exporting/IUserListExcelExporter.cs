using System.Collections.Generic;
using GEC.Attendance.Authorization.Users.Dto;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}