using System.Collections.Generic;
using Pixel.Attendance.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Pixel.Attendance.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
