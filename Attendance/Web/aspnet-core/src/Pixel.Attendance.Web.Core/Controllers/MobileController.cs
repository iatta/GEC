using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Attendance.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MobileController : AttendanceControllerBase
    {
        private readonly UserManager _userManager;
        public MobileController(UserManager userManager)
        {
            _userManager = userManager;
        }

        //[HttpPost]
        //public async Task<CheckInOutput> CheckIn([FromBody]CheckInInput input)
        //{
        //    var user =await  _userManager.Users.Include(x => x.Locations).FirstOrDefaultAsync(x => x.Id == input.UserId);
        //    if (user == null)
        //    {
        //        throw new UserFriendlyException("User Not Exist");
        //    }

        //    var output = new CheckInOutput();
        //    if (user.Locations.Count > 0)
        //    {
        //        var polygon = new Polygon();
        //        output.Status = polygon.IsPointInPolygon((int)input.Latitude, (int)input.Longitude);
        //    }
        //    else
        //    {
        //        throw new UserFriendlyException("this user doesn't has locations assigned");
        //    }

        //    output.Position = user.JobTitle.NameAr;
        //    output.UserName = user.UserName;

        //    return output;
        //}

        //[HttpPost]
        //public async Task<CheckOutOutput> CheckOut([FromBody]CheckOutInput input)
        //{
        //    var user = await _userManager.Users.Include(x => x.Locations).Where(x=>x.Locations.Where(y=>y.FromDate>=DateTime.Now)).FirstOrDefaultAsync(x => x.Id == input.UserId);
        //    if (user == null)
        //    {
        //        throw new UserFriendlyException("User Not Exist");
        //    }


        //    var output = new CheckOutOutput();
        //    if (user.Locations.Count > 0)
        //    {
        //        var polygon = new Polygon();
        //        output.Status = polygon.IsPointInPolygon((int)input.Latitude, (int)input.Longitude);
        //    }
            

        //    output.Position = user.JobTitle.NameAr;
        //    output.UserName = user.UserName;

        //    return output;
        //}
    }
}
