using Abp.AspNetCore.Mvc.Authorization;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}