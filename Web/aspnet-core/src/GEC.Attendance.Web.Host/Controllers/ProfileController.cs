using Abp.AspNetCore.Mvc.Authorization;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Web.Controllers
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