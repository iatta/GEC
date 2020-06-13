using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.GEC.Attendance.Web.Models.TokenAuth
{
    public class MobileUserModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string UserFaceId { get; set; }
        public string UserPic { get; set; }
        public DateTime LoginTime { get; set; }
        public string EmpCode { get; set; }
        public string CivilId { get; set; }

    }
}
