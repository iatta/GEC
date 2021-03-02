using System;
using System.Threading.Tasks;

namespace Pixel.Attendance.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);
        
        void SaveAsync(BinaryObject file);
        
        Task DeleteAsync(Guid id);
    }
}