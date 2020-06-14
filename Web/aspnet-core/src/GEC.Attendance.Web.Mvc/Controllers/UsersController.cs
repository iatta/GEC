using Abp.AspNetCore.Mvc.Authorization;
using GEC.Attendance.Authorization;
using GEC.Attendance.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace GEC.Attendance.Web.Controllers
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