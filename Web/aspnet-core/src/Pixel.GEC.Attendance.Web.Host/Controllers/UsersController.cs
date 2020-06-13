using Abp.AspNetCore.Mvc.Authorization;
using Pixel.GEC.Attendance.Authorization;
using Pixel.GEC.Attendance.Storage;
using Abp.BackgroundJobs;

namespace Pixel.GEC.Attendance.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}