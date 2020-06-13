using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
