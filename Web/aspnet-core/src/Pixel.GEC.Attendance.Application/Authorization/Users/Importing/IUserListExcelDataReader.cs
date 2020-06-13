using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Pixel.GEC.Attendance.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
