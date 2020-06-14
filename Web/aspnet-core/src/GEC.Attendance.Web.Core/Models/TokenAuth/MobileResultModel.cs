using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEC.Attendance.Web.Models.TokenAuth
{
    public class MobileResultModel
    {
        public MobileResultModel()
        {
            User = new MobileUserModel();
        }
        public MobileUserModel User { get; set; }
        public int ExpireInSeconds { get; set; }
        public string AccessToken { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string EncryptedAccessToken { get; set; }
    }
}
