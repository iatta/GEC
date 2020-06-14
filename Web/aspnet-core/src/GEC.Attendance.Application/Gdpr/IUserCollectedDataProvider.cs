using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
