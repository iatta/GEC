using System.Collections.Generic;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}