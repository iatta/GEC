using System.Collections.Generic;
using GEC.Attendance.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace GEC.Attendance.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
