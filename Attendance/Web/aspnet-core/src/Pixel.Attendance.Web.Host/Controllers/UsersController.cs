using Abp.AspNetCore.Mvc.Authorization;
using Pixel.Attendance.Authorization;
using Pixel.Attendance.Storage;
using Abp.BackgroundJobs;

namespace Pixel.Attendance.Web.Controllers
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