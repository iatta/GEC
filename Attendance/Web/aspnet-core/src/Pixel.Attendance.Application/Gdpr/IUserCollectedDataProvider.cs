using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
