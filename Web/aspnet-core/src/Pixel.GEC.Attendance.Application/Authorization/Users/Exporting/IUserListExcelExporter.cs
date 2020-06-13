using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Users.Dto;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}