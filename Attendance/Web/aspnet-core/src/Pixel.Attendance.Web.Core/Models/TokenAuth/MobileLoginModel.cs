using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Attendance.Web.Models.TokenAuth
{
    public class MobileLoginModel
    {
        public string UserNameOrEmailAddress { get; set; }
        public string DeviceSN { get; set; }
        public string CivilId { get; set; }
        public int LanguageId { get; set; }
    }
}
