using Abp.AspNetCore.Mvc.Authorization;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Web.Controllers
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